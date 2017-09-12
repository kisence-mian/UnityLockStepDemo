/*
  Box2DX Copyright (c) 2009 Ihar Kalasouski http://code.google.com/p/box2dx
  Box2D original C++ version Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.
*/

// The pair manager is used by the broad-phase to quickly add/remove/find pairs
// of overlapping proxies. It is based closely on code provided by Pierre Terdiman.
// http://www.codercorner.com/IncrementalSAP.txt

#define DEBUG

using System;
using Box2DX.Common;

namespace Box2DX.Collision
{
	public class Pair
	{
		[Flags]
		public enum PairStatus
		{
			PairBuffered = 0x0001,
			PairRemoved = 0x0002,
			PairFinal = 0x0004
		}

		public void SetBuffered() { Status |= PairStatus.PairBuffered; }
		public void ClearBuffered() { Status &= ~PairStatus.PairBuffered; }
		public bool IsBuffered() { return (Status & PairStatus.PairBuffered) == PairStatus.PairBuffered; }

		public void SetRemoved() { Status |= PairStatus.PairRemoved; }
		public void ClearRemoved() { Status &= ~PairStatus.PairRemoved; }
		public bool IsRemoved() { return (Status & PairStatus.PairRemoved) == PairStatus.PairRemoved; }

		public void SetFinal() { Status |= PairStatus.PairFinal; }
		public bool IsFinal() { return (Status & PairStatus.PairFinal) == PairStatus.PairFinal; }

		public object UserData;
		public ushort ProxyId1;
		public ushort ProxyId2;
		public ushort Next;
		public PairStatus Status;
	}

	public struct BufferedPair
	{
		public ushort ProxyId1;
		public ushort ProxyId2;
	}

	public abstract class PairCallback
	{
		// This should return the new pair user data. It is ok if the
		// user data is null.
		public abstract object PairAdded(object proxyUserData1, object proxyUserData2);

		// This should free the pair's user data. In extreme circumstances, it is possible
		// this will be called with null pairUserData because the pair never existed.
		public abstract void PairRemoved(object proxyUserData1, object proxyUserData2, object pairUserData);
	}

	public class PairManager
	{
		public static readonly ushort NullPair = Common.Math.USHRT_MAX;
		public static readonly ushort NullProxy = Common.Math.USHRT_MAX;
		public static readonly int TableCapacity = Settings.MaxPairs;	// must be a power of two
		public static readonly int TableMask = PairManager.TableCapacity - 1;

		public BroadPhase _broadPhase;
		public PairCallback _callback;
		public Pair[] _pairs = new Pair[Settings.MaxPairs];
		public ushort _freePair;
		public int _pairCount;

		public BufferedPair[] _pairBuffer = new BufferedPair[Settings.MaxPairs];
		public int _pairBufferCount;

		public ushort[] _hashTable = new ushort[PairManager.TableCapacity];

		public PairManager()
		{
			Box2DXDebug.Assert(Common.Math.IsPowerOfTwo((uint)PairManager.TableCapacity) == true);
			Box2DXDebug.Assert(PairManager.TableCapacity >= Settings.MaxPairs);
			for (int i = 0; i < PairManager.TableCapacity; ++i)
			{
				_hashTable[i] = PairManager.NullPair;
			}
			_freePair = 0;
			for (int i = 0; i < Settings.MaxPairs; ++i)
			{
				_pairs[i] = new Pair();//todo: need some pool here
				_pairs[i].ProxyId1 = PairManager.NullProxy;
				_pairs[i].ProxyId2 = PairManager.NullProxy;
				_pairs[i].UserData = null;
				_pairs[i].Status = 0;
				_pairs[i].Next = (ushort)(i + 1U);
			}
			_pairs[Settings.MaxPairs - 1].Next = PairManager.NullPair;
			_pairCount = 0;
			_pairBufferCount = 0;
		}

		public void Initialize(BroadPhase broadPhase, PairCallback callback)
		{
			_broadPhase = broadPhase;
			_callback = callback;
		}

		/*
		As proxies are created and moved, many pairs are created and destroyed. Even worse, the same
		pair may be added and removed multiple times in a single time step of the physics engine. To reduce
		traffic in the pair manager, we try to avoid destroying pairs in the pair manager until the
		end of the physics step. This is done by buffering all the RemovePair requests. AddPair
		requests are processed immediately because we need the hash table entry for quick lookup.

		All user user callbacks are delayed until the buffered pairs are confirmed in Commit.
		This is very important because the user callbacks may be very expensive and client logic
		may be harmed if pairs are added and removed within the same time step.

		Buffer a pair for addition.
		We may add a pair that is not in the pair manager or pair buffer.
		We may add a pair that is already in the pair manager and pair buffer.
		If the added pair is not a new pair, then it must be in the pair buffer (because RemovePair was called).
		*/
		public void AddBufferedPair(int id1, int id2)
		{
			Box2DXDebug.Assert(id1 != PairManager.NullProxy && id2 != PairManager.NullProxy);
			Box2DXDebug.Assert(_pairBufferCount < Settings.MaxPairs);

			Pair pair = AddPair(id1, id2);

			// If this pair is not in the pair buffer ...
			if (pair.IsBuffered() == false)
			{
				// This must be a newly added pair.
				Box2DXDebug.Assert(pair.IsFinal() == false);

				// Add it to the pair buffer.
				pair.SetBuffered();
				_pairBuffer[_pairBufferCount].ProxyId1 = pair.ProxyId1;
				_pairBuffer[_pairBufferCount].ProxyId2 = pair.ProxyId2;
				++_pairBufferCount;

				Box2DXDebug.Assert(_pairBufferCount <= _pairCount);
			}

			// Confirm this pair for the subsequent call to Commit.
			pair.ClearRemoved();

			if (BroadPhase.IsValidate)
			{
				ValidateBuffer();
			}
		}

		// Buffer a pair for removal.
		public void RemoveBufferedPair(int id1, int id2)
		{
			Box2DXDebug.Assert(id1 != PairManager.NullProxy && id2 != PairManager.NullProxy);
			Box2DXDebug.Assert(_pairBufferCount < Settings.MaxPairs);

			Pair pair = Find(id1, id2);

			if (pair == null)
			{
				// The pair never existed. This is legal (due to collision filtering).
				return;
			}

			// If this pair is not in the pair buffer ...
			if (pair.IsBuffered() == false)
			{
				// This must be an old pair.
				Box2DXDebug.Assert(pair.IsFinal() == true);

				pair.SetBuffered();
				_pairBuffer[_pairBufferCount].ProxyId1 = pair.ProxyId1;
				_pairBuffer[_pairBufferCount].ProxyId2 = pair.ProxyId2;
				++_pairBufferCount;

				Box2DXDebug.Assert(_pairBufferCount <= _pairCount);
			}

			pair.SetRemoved();

			if (BroadPhase.IsValidate)
			{
				ValidateBuffer();
			}
		}

		public void Commit()
		{
			int removeCount = 0;

			Proxy[] proxies = _broadPhase._proxyPool;

			for (int i = 0; i < _pairBufferCount; ++i)
			{
				Pair pair = Find(_pairBuffer[i].ProxyId1, _pairBuffer[i].ProxyId2);
				Box2DXDebug.Assert(pair.IsBuffered());
				pair.ClearBuffered();

				Box2DXDebug.Assert(pair.ProxyId1 < Settings.MaxProxies && pair.ProxyId2 < Settings.MaxProxies);

				Proxy proxy1 = proxies[pair.ProxyId1];
				Proxy proxy2 = proxies[pair.ProxyId2];

				Box2DXDebug.Assert(proxy1.IsValid);
				Box2DXDebug.Assert(proxy2.IsValid);

				if (pair.IsRemoved())
				{
					// It is possible a pair was added then removed before a commit. Therefore,
					// we should be careful not to tell the user the pair was removed when the
					// the user didn't receive a matching add.
					if (pair.IsFinal() == true)
					{
						_callback.PairRemoved(proxy1.UserData, proxy2.UserData, pair.UserData);
					}

					// Store the ids so we can actually remove the pair below.
					_pairBuffer[removeCount].ProxyId1 = pair.ProxyId1;
					_pairBuffer[removeCount].ProxyId2 = pair.ProxyId2;
					++removeCount;
				}
				else
				{
					Box2DXDebug.Assert(_broadPhase.TestOverlap(proxy1, proxy2) == true);

					if (pair.IsFinal() == false)
					{
						pair.UserData = _callback.PairAdded(proxy1.UserData, proxy2.UserData);
						pair.SetFinal();
					}
				}
			}

			for (int i = 0; i < removeCount; ++i)
			{
				RemovePair(_pairBuffer[i].ProxyId1, _pairBuffer[i].ProxyId2);
			}

			_pairBufferCount = 0;

			if (BroadPhase.IsValidate)
			{
				ValidateTable();
			}
		}

		private Pair Find(int proxyId1, int proxyId2)
		{
			if (proxyId1 > proxyId2)
				Common.Math.Swap<int>(ref proxyId1, ref proxyId2);

			uint hash = (uint)(Hash((uint)proxyId1, (uint)proxyId2) & PairManager.TableMask);

			return Find(proxyId1, proxyId2, hash);
		}

		private Pair Find(int proxyId1, int proxyId2, uint hash)
		{
			int index = _hashTable[hash];

			while (index != PairManager.NullPair && Equals(_pairs[index], proxyId1, proxyId2) == false)
			{
				index = _pairs[index].Next;
			}

			if (index == PairManager.NullPair)
			{
				return null;
			}

			Box2DXDebug.Assert(index < Settings.MaxPairs);

			return _pairs[index];
		}

		// Returns existing pair or creates a new one.
		private Pair AddPair(int proxyId1, int proxyId2)
		{
			if (proxyId1 > proxyId2)
				Common.Math.Swap<int>(ref proxyId1, ref proxyId2);

			int hash = (int)(Hash((uint)proxyId1, (uint)proxyId2) & PairManager.TableMask);

			Pair pair = Find(proxyId1, proxyId2, (uint)hash);
			if (pair != null)
			{
				return pair;
			}

			Box2DXDebug.Assert(_pairCount < Settings.MaxPairs && _freePair != PairManager.NullPair);

			ushort pairIndex = _freePair;
			pair = _pairs[pairIndex];
			_freePair = pair.Next;

			pair.ProxyId1 = (ushort)proxyId1;
			pair.ProxyId2 = (ushort)proxyId2;
			pair.Status = 0;
			pair.UserData = null;
			pair.Next = _hashTable[hash];

			_hashTable[hash] = pairIndex;

			++_pairCount;

			return pair;
		}

		// Removes a pair. The pair must exist.
		private object RemovePair(int proxyId1, int proxyId2)
		{
			Box2DXDebug.Assert(_pairCount > 0);

			if (proxyId1 > proxyId2) 
				Common.Math.Swap<int>(ref proxyId1, ref proxyId2);

			int hash = (int)(Hash((uint)proxyId1, (uint)proxyId2) & PairManager.TableMask);

			//uint16* node = &m_hashTable[hash];
			ushort node = _hashTable[hash];
			bool ion = false;
			int ni = 0;
			while (node != PairManager.NullPair)
			{
				if (Equals(_pairs[node], proxyId1, proxyId2))
				{
					//uint16 index = *node;
					//*node = m_pairs[*node].next;

					ushort index = node;
					node = _pairs[node].Next;										
					if (ion)
						_pairs[ni].Next = node;
					else
					{
						_hashTable[hash] = node;
					}

					Pair pair = _pairs[index];
					object userData = pair.UserData;

					// Scrub
					pair.Next = _freePair;
					pair.ProxyId1 = PairManager.NullProxy;
					pair.ProxyId2 = PairManager.NullProxy;
					pair.UserData = null;
					pair.Status = 0;

					_freePair = index;
					--_pairCount;
					return userData;
				}
				else
				{
					//node = &m_pairs[*node].next;
					ni = node;
					node = _pairs[ni].Next;
					ion = true;
				}
			}

			Box2DXDebug.Assert(false);
			return null;
		}

		private void ValidateBuffer()
		{
#if DEBUG
			Box2DXDebug.Assert(_pairBufferCount <= _pairCount);

			//std::sort(m_pairBuffer, m_pairBuffer + m_pairBufferCount);
			BufferedPair[] tmp = new BufferedPair[_pairBufferCount];
			Array.Copy(_pairBuffer, 0, tmp, 0, _pairBufferCount);
			Array.Sort<BufferedPair>(tmp, BufferedPairSortPredicate);
			Array.Copy(tmp, 0, _pairBuffer, 0, _pairBufferCount);

			for (int i = 0; i < _pairBufferCount; ++i)
			{
				if (i > 0)
				{
					Box2DXDebug.Assert(Equals(_pairBuffer[i], _pairBuffer[i - 1]) == false);
				}

				Pair pair = Find(_pairBuffer[i].ProxyId1, _pairBuffer[i].ProxyId2);
				Box2DXDebug.Assert(pair.IsBuffered());

				Box2DXDebug.Assert(pair.ProxyId1 != pair.ProxyId2);
				Box2DXDebug.Assert(pair.ProxyId1 < Settings.MaxProxies);
				Box2DXDebug.Assert(pair.ProxyId2 < Settings.MaxProxies);

				Proxy proxy1 = _broadPhase._proxyPool[pair.ProxyId1];
				Proxy proxy2 = _broadPhase._proxyPool[pair.ProxyId2];

				Box2DXDebug.Assert(proxy1.IsValid == true);
				Box2DXDebug.Assert(proxy2.IsValid == true);
			}
#endif
		}

		private void ValidateTable()
		{
#if DEBUG
			for (int i = 0; i < PairManager.TableCapacity; ++i)
			{
				ushort index = _hashTable[i];
				while (index != PairManager.NullPair)
				{
					Pair pair = _pairs[index];
					Box2DXDebug.Assert(pair.IsBuffered() == false);
					Box2DXDebug.Assert(pair.IsFinal() == true);
					Box2DXDebug.Assert(pair.IsRemoved() == false);

					Box2DXDebug.Assert(pair.ProxyId1 != pair.ProxyId2);
					Box2DXDebug.Assert(pair.ProxyId1 < Settings.MaxProxies);
					Box2DXDebug.Assert(pair.ProxyId2 < Settings.MaxProxies);

					Proxy proxy1 = _broadPhase._proxyPool[pair.ProxyId1];
					Proxy proxy2 = _broadPhase._proxyPool[pair.ProxyId2];

					Box2DXDebug.Assert(proxy1.IsValid == true);
					Box2DXDebug.Assert(proxy2.IsValid == true);

					Box2DXDebug.Assert(_broadPhase.TestOverlap(proxy1, proxy2) == true);

					index = pair.Next;
				}
			}
#endif
		}

		// Thomas Wang's hash, see: http://www.concentric.net/~Ttwang/tech/inthash.htm
		// This assumes proxyId1 and proxyId2 are 16-bit.
		private uint Hash(uint proxyId1, uint proxyId2)
		{
			uint key = (proxyId2 << 16) | proxyId1;
			key = ~key + (key << 15);
			key = key ^ (key >> 12);
			key = key + (key << 2);
			key = key ^ (key >> 4);
			key = key * 2057;
			key = key ^ (key >> 16);
			return key;
		}

		private bool Equals(Pair pair, int proxyId1, int proxyId2)
		{
			return pair.ProxyId1 == proxyId1 && pair.ProxyId2 == proxyId2;
		}

		private bool Equals(ref BufferedPair pair1, ref BufferedPair pair2)
		{
			return pair1.ProxyId1 == pair2.ProxyId1 && pair1.ProxyId2 == pair2.ProxyId2;
		}

		public static int BufferedPairSortPredicate(BufferedPair pair1, BufferedPair pair2)
		{
			if (pair1.ProxyId1 < pair2.ProxyId1)
				return 1;
			else if (pair1.ProxyId1 > pair2.ProxyId1)
				return -1;
			else
			{
				if (pair1.ProxyId2 < pair2.ProxyId2)
					return 1;
				else if (pair1.ProxyId2 > pair2.ProxyId2)
					return -1;
			}

			return 0;
		}
	}
}