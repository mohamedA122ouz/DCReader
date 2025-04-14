import os
import shutil
import subprocess
import zipfile
import requests

def is_installed(command):
    return shutil.which(command) is not None

def install_ffmpeg_manual():
    if is_installed("ffmpeg"):
        print("✅ FFmpeg is already installed.")
        return

    print("Installing FFmpeg manually...")

    url = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip"
    zip_path = "ffmpeg.zip"
    extract_path = "ffmpeg"

    # Download the zip
    response = requests.get(url, stream=True)
    with open(zip_path, "wb") as f:
        for chunk in response.iter_content(chunk_size=8192):
            if chunk:
                f.write(chunk)

    # Extract
    with zipfile.ZipFile(zip_path, 'r') as zip_ref:
        zip_ref.extractall(extract_path)

    # Find and add ffmpeg.exe directory to PATH for this session
    ffmpeg_bin_path = None
    for root, dirs, files in os.walk(extract_path):
        if "ffmpeg.exe" in files:
            ffmpeg_bin_path = root
            break

    if ffmpeg_bin_path:
        os.environ["PATH"] += os.pathsep + ffmpeg_bin_path
        print(f"✅ FFmpeg installed and temporarily added to PATH: {ffmpeg_bin_path}")
    else:
        print("❌ FFmpeg binary not found!")

    # Clean up zip
    os.remove(zip_path)

def install_dotnet_manual():
    if is_installed("dotnet"):
        print("✅ .NET SDK is already installed.")
        return

    print("Installing .NET SDK manually...")

    # Download .NET SDK installer (adjust version if needed)
    url = "https://download.visualstudio.microsoft.com/download/pr/dc4f3729-54f7-4e79-9c31-d7ab7284592a/634e7c5ef82ce1c0c7b8e93d979d3d2e/dotnet-sdk-8.0.100-win-x64.exe"
    installer = "dotnet-installer.exe"

    response = requests.get(url, stream=True)
    with open(installer, "wb") as f:
        for chunk in response.iter_content(chunk_size=8192):
            if chunk:
                f.write(chunk)

    # Install silently
    subprocess.run([installer, "/quiet", "/norestart"], check=True)

    print("✅ .NET SDK installed.")
    os.remove(installer)

def run_server():
    project_path = "DcReader"
    if os.path.exists(project_path):
        print("Starting the ASP.NET MVC server...")
        os.chdir(project_path)

        server_process = subprocess.Popen(["dotnet", "run"])
        server_process.communicate()

        print("Server has stopped.")
    else:
        print("❌ Project directory not found! Run setup first.")

# Run the setup process
install_ffmpeg_manual()
install_dotnet_manual()
run_server()
