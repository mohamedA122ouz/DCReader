import os
import shutil
import subprocess

def is_installed(command):
    return shutil.which(command) is not None

def install_winget():
    if not is_installed("winget"):
        print("winget not found. Installing...")
        os.system("curl -L -o winget.msixbundle https://aka.ms/getwinget")
        os.system("powershell -Command Add-AppxPackage winget.msixbundle")
    else:
        print("✅ winget is already installed.")

def install_ffmpeg():
    if not is_installed("ffmpeg"):
        print("Installing FFmpeg...")
        os.system("winget install -e --id Gyan.FFmpeg")
    else:
        print("✅ FFmpeg is already installed.")

def install_dotnet():
    if not is_installed("dotnet"):
        print("Installing .NET SDK...")
        os.system("winget install -e --id Microsoft.DotNet.SDK.8")
    else:
        print("✅ .NET SDK is already installed.")


def run_server():
    project_path = "DcReader"
    if os.path.exists(project_path):
        print("Starting the ASP.NET MVC server...")
        os.chdir(project_path)

        # Run the server using subprocess to keep the terminal open
        server_process = subprocess.Popen(["dotnet", "run"])

        # Wait for the server process to exit (this keeps the terminal open)
        server_process.communicate()

        print("Server has stopped. Press any key to exit.")
    else:
        print("❌ Project directory not found! Run setup first.")

# Run the setup process
install_winget()
install_ffmpeg()
install_dotnet()
run_server()