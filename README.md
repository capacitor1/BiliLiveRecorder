# BiliLiveRecorder

B站直播录制器（强制原画）

- 无需登录或cookie，直接访问B站直播HLS/FMP4原画流，强制访问原画（删除URL中的无关后缀实现）

- 目前不可录制弹幕消息流和不提供FMP4流的直播间。

### 使用方法

- 点击左上角 `房间` `添加` 然后输入房间ID（例子：`25021721`，仅数字部分）

<img width="371" height="101" alt="{4B1AEF21-ED6D-4861-AA65-FC58F19B60C3}" src="https://github.com/user-attachments/assets/f3cdaa0b-4870-4d24-9c87-dd35b1a418af" />

- 确认后左侧列表会出现该ID（如输入的ID是短ID，则会自动跳转至真ID）和当前直播间名称。

<img width="235" height="30" alt="{9509FCC3-C8DE-44EE-828B-569C9C2DAD7F}" src="https://github.com/user-attachments/assets/808290a5-9929-4c5c-9ca7-a3e5ec9032e7" />

- 选中此项并右键，点击 `启动录制` 则开始自动监控房间。

- 如果直播开始，则自动开始录制。右键房间，点击 `在本地文件夹中打开` 可查看录制中的文件[^1]

- 在直播开始和结束时，系统右下角会弹窗提示。如果不需要提示，则需要从系统设置中关闭软件的通知，录制器内没有设置项。

- 录制完成后，文件可正常在本地播放，也可先进行转封装合并[^2] 后保留单独的视频文件[^3]。

<img width="347" height="84" alt="{050A3766-4E1C-42CB-B4B6-F3C0EA410E11}" src="https://github.com/user-attachments/assets/a7abad36-b926-42da-bb7e-15d6e6f9fa85" />

### 录制的文件

- 单次录制任务会以录制开始的时间作为文件夹名称。文件夹内文件如下示例：

<img width="177" height="333" alt="{B3C9F1FA-8650-40F9-B539-820F6BF79989}" src="https://github.com/user-attachments/assets/37baa067-8571-451a-91cd-9ea5d3c42ea6" />

`_Cover.jpg` 为直播开始前抓取的封面图。

`_Index.m3u8` 为视频文件*索引*。打开该文件可播放直播。

`_Index.m3u8.txt` 为录制使用的M3U8源URL。此文件一般无用，可直接删除。但如果发现录制器报错`Invalid M3U8`或者录制的视频有明显模糊不清晰或出现大量crc校验失败等错误，可手动检查该URL是否不符合规定（如路径错误、服务器Host不存在或分配到的服务器实际上不稳定）

`_Info.md` 为直播间基本信息（包括标题、简介），但不包括UP主信息。

`hxxxxxx.m4s` 为视频头文件（`#EXT-X-MAP`），如果有多个，则本次直播切换了多次分辨率。

`xxxxxx.m4s` 为视频分片，如果出现`xxxxxx.m4s.crc32failed`文件，则该分片与服务器上的CRC32不一致，分片可能损坏；如果在录制视频的最后几个分片发现`xxxxxx.m4s.unverified`，且录制结束的原因是`Record repired`，则可能是网络带宽不足，录制速度过于缓慢导致分片失效，这几个分片没有写入m3u8中也可能没有下载成功。

### 软件已知缺陷

- 无法录制无FMP4流的直播。此种情况会使录制报错`Invalid M3U8`，并且只能手动停止录制。

- Http请求可能出现OK数量大于Send数量的情况：

<img width="352" height="24" alt="{4B30F4B1-9AD2-4D85-BAAF-B29104AD1C53}" src="https://github.com/user-attachments/assets/2c912244-c418-4acc-b4a6-bf126ea6132d" />

[^1]:录制的直播为M3U8+HLS格式，在录制未结束时，使用 [MPV](https://mpv.io/) 打开`_Index.m3u8`文件可直接观看直播（同步进度）。也可用FFMPEG直接对m3u8进行实时合并。*注意，使用此法本地看直播必须保证网络通畅，录制速度大于等于直播进行速度，录制过慢而播放更快会使软件认为已经播放完毕而自动退出。*

[^2]:有些主播会在直播过程中切换分辨率（会发现M3U8中有 `#EXT-X-DISCONTINUITY` 标识及多个文件头）。此时的视频不能转换编码格式，FFMPEG在分辨率切换之前就会结束转码自动退出。如果手动删除标识再强制转换会导致视频花屏。遇到此种情况建议另行处理（保留原始分片或者使用其他方法转封装）。

[^3]:由于B站部分直播码率较低，输出的分片文件平均体积较小，如果保留大量分片可能影响**机械硬盘**性能（复制文件或者使用索引工具搜索磁盘时速度较慢）。
