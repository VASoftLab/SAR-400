# SAR-400 Python code

## Required steps before launch:

Preinstall Python to your operating system. It's better to install Python 3.7+ version.

## Python installation guide:

  ### Windows:
  -The most easiest way is to get Python is to install first package manager [Chocolatey](https://chocolatey.org) and then install python:
> 'choco install python --version 3.7.2' 
as described here: https://chocolatey.org/packages/python/3.7.2
It worth to mention that Chocolatey already set python to your Path variables for you to use **__python__** from command line/terminal
  -Another option is to install Python in the common way downloaded from [official Python site](https://www.python.org/downloads/) .  Don't forget to set up in the Path variables python location to use Python from command line/terminal (Right click on **My Pc** -> Settings -> Advanced settings -> Environment variables -> Edit **Path** -> Add the location to installed python).

  ### Linux:

todo add documentation for Linux
  
  ### Mac os:
  -The same as for Windows the easiest way to get installed Python for Mac Os is use package-manager [Homebrew](https://brew.sh/index_ru) :
 > 'brew install python3'
   Good article how to set up Brew and Python3 -> https://www.digitalocean.com/community/tutorials/how-to-install-python-3-and-set-up-a-local-programming-environment-on-macos
    todo add vanilla Python installation for Mac



To execute program pass csv file to resources folder and execute from SAR-400-Python folder in console/terminal following command:

python -m service.robot_manipulator csv_filename.csv

where csv_filename is the name of csv with commands at resources folder
