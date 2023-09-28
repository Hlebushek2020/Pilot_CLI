# Pilot CLI

## Требования
- [.NET Runtime 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Настройки
Файл настроек будет создан при первом запуске программы. В дальнейшем что бы узнать путь к нему воспользуйтесь командой `settings`\
Описание ключей:
- `contexts` - Объект представляющий список всех доступных подключений. Где ключем является уникальное название подключения, а значением объект представляющий подключение. Его ключи приведены на следующем уровне. 
  - `pilot_server_url` - Адрес сервера Pilot.
  - `pilot_server_database` - База данных.
  - `pilot_server_username` - Учетная запись пользователя для базы данных заданной выше.
  - `pilot_server_password` - Пароль от учетной записи.
  - `pilot_server_license_code` - [Тип лицензии.](#коды-лицензий)
- `command_signature_color` - [Цвет вывода сигнатур комманд.](#доступные-цвета)
- `command_color` - [Цвет ввода комманд.](#доступные-цвета)
- `other_text_color` - [Цвет остального текста.](#доступные-цвета)

### Коды лицензий
- `100` - Pilot ICE
- `101` - Pilot ECM
- `103` - Pilot Enterprise

### Доступные цвета
Строковое значение|Цифровое значение|Цвет
-|-|-
`"Black"`|`0`| <div style="height: 16px; width: 16px; background-color:#000000"></div>
`"DarkBlue"`|`1`|
`"DarkGreen"`|`2`|
`"DarkCyan"`|`3`|
`"DarkRed"`|`4`|
`"DarkMagenta"`|`5`|
`"DarkYellow"`|`6`|
`"Gray"`|`7`|
`"DarkGray"`|`8`|
`"Blue"`|`9`|
`"Green"`|`10`|
`"Cyan"`|`11`|
`"Red"`|`12`|
`"Magenta"`|`13`|
`"Yellow"`|`14`|
`"White"`|`15`|