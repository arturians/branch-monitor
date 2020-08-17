С помощью данного проекта можно отслеживать "подвисшие" ветки.  
Для этого используются команды git:  
1. git fetch --prune  
2. git branch -r --merged origin/master
3. git push origin :<merged_branch_name>
4. git remote prune origin <merged_branch_name>
5. git branch -rv
6. git log -n 1 --format="%an %ae %D %cr %ct" <remote_branch>

Команда #6 выполняется для каждой ветки, полученной командой #5.  
Таким образом получаем информацию о ветке (автор, его email, дата обновления ветки).  
Эта информация отправляется в Google-таблицу через API.

Для работы Google API нужен файл credentials.json, который не включен в данный проект.  
Если вы хотите сгенерировать для себя такой файл, то перейдите по ссылке
https://developers.google.com/sheets/api/quickstart/dotnet
и нажмите на Enable Google Sheets Api.

Далее добавьте сгенерированный файл в проект.