# Инструкция по переименованию коммитов

## Проблемные коммиты (с неправильной кодировкой или на русском):

1. `5127416` - "ефакторинг" → должно быть: "Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events"
2. `0d30474` - "еализована" → должно быть: "Implement multi-user system with profile support"
3. `304dcce` - "РџРµСЂРµРїРёСЃР°Р» РєРѕРґ TodoList РїРѕРґ РєР»Р°СЃСЃС‹" → должно быть: "Refactor TodoList to use classes (practical assignment lecture 6 OOP)"

## Способ 1: Интерактивный rebase (рекомендуется)

1. Отключите пейджер временно:
   ```powershell
   $env:GIT_PAGER=''
   ```

2. Запустите интерактивный rebase (для последних 15 коммитов):
   ```powershell
   git rebase -i HEAD~15
   ```

3. В открывшемся редакторе найдите проблемные коммиты и замените `pick` на `reword` (или `r`):
   ```
   reword 5127416 ефакторинг
   reword 0d30474 еализована
   reword 304dcce РџРµСЂРµРїРёСЃР°Р» РєРѕРґ TodoList РїРѕРґ РєР»Р°СЃСЃС‹
   ```

4. Сохраните и закройте редактор (в vim: `:wq`, в nano: Ctrl+X, затем Y)

5. Для каждого коммита с `reword` откроется редактор - введите новое сообщение:
   - Для 5127416: `Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events`
   - Для 0d30474: `Implement multi-user system with profile support`
   - Для 304dcce: `Refactor TodoList to use classes (practical assignment lecture 6 OOP)`

6. После завершения rebase отправьте изменения:
   ```powershell
   git push --force-with-lease origin feature/todolist
   ```

## Способ 2: Использование git commit --amend (только для последнего коммита)

Если нужно переименовать только последний коммит:
```powershell
git commit --amend -m "Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events"
git push --force-with-lease origin feature/todolist
```

## ВАЖНО:
- `--force-with-lease` безопаснее, чем `--force`, так как проверяет, что удаленная ветка не изменилась
- Если другие люди работают с этой веткой, предупредите их перед force push
- После force push другим разработчикам нужно будет выполнить `git fetch` и `git reset --hard origin/feature/todolist`

