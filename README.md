# BiliLiveRecorder
## B站直播下载器
支持：
- **无视B站API不返回原画**，强制下载原画流
- 支持下载信息流（有bug并且只支持原始数据，仅参考，之后修）
- 多个房间（理论上支持的房间数量是ListView行数的上限）同时运行+独立控制，可以自由启停某个房间的录制
- 报错自动重试处理+日志记录（二进制日志，仅分析错误用），避免手动下载时的麻烦
- 自动监视房间，一开播就开始下载，监控间隔可调
- **高度可设置**，大部分参数都可由用户自定义，即使发现默认的监视间隔和写入策略不好，也能自己调整
- **自动保存ID列表和设置**，不用担心关闭软件无法保存设置项目。

目前仍存在缺陷：
- 部分直播流B站内部并没有处理为FMP4格式，导致软件会一直报错404。

使用方法：
- 打开软件，左上角Stream按钮，Add Stream，输入ID，会自动添加（如果ID正确则会自动显示出直播间标题）。
- 右键这个ID，点击Start开始运行，如果可下载则直接开始下载，不可下载则继续监控。
- 如果开始下载，右侧日志列表会开始刷新，下方相关数据也会实时（每隔1s刷新）显示，可根据这些信息判断当前流的下载情况。
- 无论是什么后续操作，右键相关项目即可。
- **关闭软件防手残**，点关闭不会直接退出，而是先询问是否退出。

注意事项：
1. 下载的文件分别是 fmp4视频，mrec数据（弹幕）流，minf元信息，cjpg封面。打开方式分别是：fmp4视频直接用视频播放器（**推荐MPV**）播放，minf使用markdown查看器查看，cjpg是正常jpg图片。*为了统一成4字符后缀所以看起来都无法直接打开。*
2. 目前mrec是二进制数据，无法正常打开，如果不需要则删除即可，需要则：1.等待后续版本支持转换mrec数据到文本。2.自行查看B站数据流文档并解析文件。

> 补注：为什么推荐MPV播放视频：因为FMP4视频格式本身比较特殊，以及B站返回视频的两个特性：
> - 特性1：同一场直播中允许有不同的分辨率，主播可随时切换分辨率。此时，普通播放器通常无法播放，或者播放的视频画面是拉伸的，而MPV在这一点没有问题，MPV的窗口大小会自动切换。
> - 特性2：B站返回的视频时长和直播持续时长一致，这导致如果不是从第0s开始录制，那么视频时长就是错的，一般的播放器遇到这个情况，进度条组件就寄了，而MPV则会按照原始时长作为进度条。（注意如果视频不是从第0s开始录制，那么MPV显示的进度条开始的那部分无法拖动。）
