# TChat.Coder.API
API для кодера TChat - кодирует текст, файл, и т.д - с помощью пароля. Используется в проекте TChat
## Как использовать?

``` C#
TChat.Coder.API.Main coder = new TChat.Coder.API.Main();
//...

string a =   coder.Encode("blablabla", "pass"); //Шифровка
string b =   coder.Decode(a, "pass"); //Дешифровка

```
