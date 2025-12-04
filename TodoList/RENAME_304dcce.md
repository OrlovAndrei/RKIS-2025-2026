# Переименование коммита 304dcce

## Текущее название (неправильная кодировка):
`РџРµСЂРµРїРёСЃР°Р» РєРѕРґ TodoList РїРѕРґ РєР»Р°СЃСЃС‹ (РїСЂР°РєС‚РёС‡РµСЃРєРѕРµ Р·Р°РґР°РЅРёРµ Р»РµРєС†РёСЏ 6 РћРћРџ)`

## Новое название:
`Refactor TodoList to use classes (lecture 6 OOP practical assignment)`

## Команды для выполнения:

1. Запустите интерактивный rebase:
```powershell
git rebase -i 304dcce^
```

2. В открывшемся редакторе найдите строку с `304dcce` и замените `pick` на `reword` (или просто `r`)

3. Сохраните и закройте редактор (в vim: нажмите `Esc`, затем введите `:wq` и нажмите Enter)

4. Откроется редактор для ввода нового сообщения - введите:
```
Refactor TodoList to use classes (lecture 6 OOP practical assignment)
```

5. Сохраните и закройте редактор

6. После завершения rebase отправьте изменения:
```powershell
git push --force-with-lease origin feature/todolist
```

