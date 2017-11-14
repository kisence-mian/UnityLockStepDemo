# UnityFrameSyncDemo
基于Unity做的帧同步Demo  
底层使用了[我的Unity框架](https://github.com/GaoKaiHaHa/MyUnityFrameWork)和[SupperSocket](http://www.supersocket.net/)  
ECS架构

构建目标：  
　　1.前后端能统一语言的框架，以至于在前端写好的游戏逻辑，拿到后端就可以直接使用。   
　　2.在框架层面解决同步问题，在此之上写游戏逻辑的时候不需要再考虑游戏同步的问题。  

架构思路见我的[博客](https://www.kisence.com/2017/11/12/guan-yu-zheng-tong-bu-de-xie-xin-de/)

### 可配置项(在项目的App.config中配置)  

    IsDebug => Debug开关  

    DataBaseURL = 数据库地址  
    DataBaseUser = 数据库用户名  
    DataBasePassword = 数据库密码  
    DataBaseName = 数据库名  

    RoomPeopleNum = 房间的人数  
    UpdateInterval = 帧更新间隔(毫秒)  
