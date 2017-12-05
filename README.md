# UnityLockStepDemo
基于Unity做的帧同步Demo  
底层使用了[我的Unity框架](https://github.com/GaoKaiHaHa/MyUnityFrameWork)和[SupperSocket](http://www.supersocket.net/)  
ECS架构  
该框架还在开发中

### 构建目标：  
　　1.前后端能统一语言的框架，以至于在前端写好的游戏逻辑，拿到后端就可以直接使用。   
　　2.在框架层面解决同步问题，在此之上写游戏逻辑的时候不需要再考虑游戏同步的问题。  

架构思路见我的[博客](https://www.kisence.com/2017/11/12/guan-yu-zheng-tong-bu-de-xie-xin-de/)

### 客户端目录
　　Script/SyncFrameWork 存放前端同步框架  
　　Script/SyncClientLogic 存放游戏表现层逻辑  
　　Script/SyncGameLogic 存放游戏数据层逻辑，这里的代码要与服务器一致  

### 服务器目录
　　SyncGameLogic 存放游戏数据层逻辑  
　　ServiceFrameWork目录下是C#服务器框架  
　　LockStepFrameWork目录下是帧同步框架  
　　Data是配置表目录  
　　Network是网络协议目录  
　　Generate是自动生成的代码目录  
  
### 客户端工具
　　见[前端框架](https://github.com/GaoKaiHaHa/MyUnityFrameWork)
  
### 服务器工具

　　Tool_GenerateAnalysisCode 生成协议解析代码  
　　Tool_ClearAnalysisCode 清除协议解析代码  
　　Tool_CSharp2Protocol 把c#类生成对应协议  
　　Tool_Protocol2Csharp 把协议生成对应的C#类  
　　Tool_GenerateDataClass 把数据表转换成对应的c#类，用法与前端相同  


### 服务器可配置项(在项目的App.config中配置)  

　　IsDebug => Debug开关  

　　DataBaseURL = 数据库地址  
　　DataBaseUser = 数据库用户名  
　　DataBasePassword = 数据库密码  
　　DataBaseName = 数据库名  

　　RoomPeopleNum = 房间的人数  
　　UpdateInterval = 帧更新间隔(毫秒)  
