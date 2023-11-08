# Pilot CLI

![image](https://github.com/Hlebushek2020/Pilot_CLI/assets/63193749/19426cfa-3e61-4405-aec3-9e1276be28e0)


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
`"Black"`|`0`|[#000000](https://color2.ru/000000)
`"DarkBlue"`|`1`|[#00008b](https://color2.ru/00008b)
`"DarkGreen"`|`2`|[#006400](https://color2.ru/006400)
`"DarkCyan"`|`3`|[#008b8b](https://color2.ru/008b8b)
`"DarkRed"`|`4`|[#8b0000](https://color2.ru/8b0000)
`"DarkMagenta"`|`5`|[#8b008b](https://color2.ru/8b008b)
`"DarkYellow"`|`6`|[#808000](https://color2.ru/808000)
`"Gray"`|`7`|[#808080](https://color2.ru/808080)
`"DarkGray"`|`8`|[#a9a9a9](https://color2.ru/a9a9a9)
`"Blue"`|`9`|[#0000ff](https://color2.ru/0000ff)
`"Green"`|`10`|[#008000](https://color2.ru/008000)
`"Cyan"`|`11`|[#00ffff](https://color2.ru/00ffff)
`"Red"`|`12`|[#ff0000](https://color2.ru/ff0000)
`"Magenta"`|`13`|[#ff00ff](https://color2.ru/ff00ff)
`"Yellow"`|`14`|[#ffff00](https://color2.ru/ffff00)
`"White"`|`15`|[#ffffff](https://color2.ru/ffffff)
