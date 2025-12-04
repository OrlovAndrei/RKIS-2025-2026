# Скрипт для переименования коммитов
# Использование: запустите интерактивный rebase вручную

Write-Host "Для переименования коммитов выполните следующие команды:"
Write-Host ""
Write-Host "1. git rebase -i HEAD~10  (или больше, в зависимости от количества коммитов)"
Write-Host ""
Write-Host "2. В открывшемся редакторе замените 'pick' на 'reword' для коммитов, которые нужно переименовать"
Write-Host ""
Write-Host "3. Сохраните и закройте редактор"
Write-Host ""
Write-Host "4. Для каждого коммита с 'reword' откроется редактор - введите новое сообщение"
Write-Host ""
Write-Host "5. После завершения: git push --force-with-lease origin feature/todolist"
Write-Host ""
Write-Host "Коммиты для переименования:"
Write-Host "  - 5127416: Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events"
Write-Host "  - 0d30474: Implement multi-user system with profile support"
Write-Host "  - 304dcce: Refactor TodoList to use classes (practical assignment lecture 6 OOP)"

