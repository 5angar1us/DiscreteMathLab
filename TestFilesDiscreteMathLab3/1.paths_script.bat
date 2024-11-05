@echo off
setlocal enabledelayedexpansion

:: Устанавливаем кодировку для корректного отображения русских символов
chcp 65001 > nul

:: Устанавливаем имя файла для сохранения путей
set "outputfile=1.paths.txt"

:: Очищаем файл вывода, если он уже существует
if exist "%outputfile%" del "%outputfile%"

:: Находим все файлы в текущей директории и поддиректориях
for /r %%F in (*) do (
    echo %%F>> "%outputfile%"
	:: Добавляем пустую строку после каждого пути
    echo.>> "%outputfile%"  
)



echo Пути к файлам сохранены в файле %outputfile%

:: Возвращаем стандартную кодировку
chcp 866>nul
pause