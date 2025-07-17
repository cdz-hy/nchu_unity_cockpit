from moviepy import VideoFileClip
import os

# 视频文件列表
videos = [
    "images/media3.mp4",
    "images/media4.mp4", 
    "images/media8.mp4",
    "images/media9.mp4",
    "images/media10.mp4",
    "images/media11.mp4"
]

# 转换设置
fps = 10  # GIF帧率
max_width = 480  # 最大宽度

for video_path in videos:
    if not os.path.exists(video_path):
        print(f"跳过 {video_path} - 文件不存在")
        continue
        
    gif_path = video_path.replace(".mp4", ".gif")
    print(f"转换 {video_path} -> {gif_path}")
    
    try:
        # 加载视频
        clip = VideoFileClip(video_path)
        
        # 调整大小（保持宽高比）
        if clip.w > max_width:
            ratio = max_width / clip.w
            clip = clip.resized(ratio)
        
        # 转换为GIF
        clip.write_gif(gif_path, fps=fps)
        clip.close()
        
        # 检查文件大小
        size_mb = os.path.getsize(gif_path) / (1024 * 1024)
        print(f"  完成: {size_mb:.1f} MB")
        
        # 如果太大，降低质量
        if size_mb > 10:
            print(f"  文件太大，重新转换...")
            clip = VideoFileClip(video_path)
            if clip.w > max_width // 2:
                ratio = (max_width // 2) / clip.w
                clip = clip.resized(ratio)
            clip.write_gif(gif_path, fps=fps // 2)
            clip.close()
            size_mb = os.path.getsize(gif_path) / (1024 * 1024)
            print(f"  优化后: {size_mb:.1f} MB")
            
    except Exception as e:
        print(f"  错误: {e}")

print("\n转换完成！")
