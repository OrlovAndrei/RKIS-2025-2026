# Простой скрипт для переименования коммитов
# Меняет ТОЛЬКО сообщения коммитов, код не трогает

Write-Host "=== Переименование сообщений коммитов ===" -ForegroundColor Green
Write-Host "ВНИМАНИЕ: Это изменит только сообщения коммитов, код останется без изменений!" -ForegroundColor Yellow
Write-Host ""

# Список коммитов для переименования (хеш -> новое сообщение)
$commitsToRename = @{
    "5127416" = "Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events"
    "0d30474" = "Implement multi-user system with profile support"
    "304dcce" = "Refactor TodoList to use classes (practical assignment lecture 6 OOP)"
}

Write-Host "Коммиты для переименования:" -ForegroundColor Cyan
foreach ($hash in $commitsToRename.Keys) {
    Write-Host "  $hash -> $($commitsToRename[$hash])" -ForegroundColor White
}

Write-Host ""
$confirm = Read-Host "Продолжить? (y/n)"

if ($confirm -ne "y") {
    Write-Host "Отменено." -ForegroundColor Yellow
    exit
}

Write-Host ""
Write-Host "Для переименования выполните следующие шаги:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Запустите интерактивный rebase:" -ForegroundColor Cyan
Write-Host "   git rebase -i HEAD~15" -ForegroundColor White
Write-Host ""
Write-Host "2. В открывшемся редакторе найдите коммиты по хешам выше" -ForegroundColor Cyan
Write-Host "3. Замените 'pick' на 'reword' (или просто 'r') для этих коммитов" -ForegroundColor Cyan
Write-Host "4. Сохраните и закройте редактор (в vim: нажмите Esc, затем :wq и Enter)" -ForegroundColor Cyan
Write-Host "5. Для каждого коммита с 'reword' введите новое сообщение из списка выше" -ForegroundColor Cyan
Write-Host "6. После завершения rebase выполните:" -ForegroundColor Cyan
Write-Host "   git push --force-with-lease origin feature/todolist" -ForegroundColor White
Write-Host ""
Write-Host "ВАЖНО: После rebase нужно будет сделать push с правильным аккаунтом GitHub!" -ForegroundColor Red

