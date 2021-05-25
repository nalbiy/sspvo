В первую очередь необходимо скачать и установить КриптоПро CSP. https://www.cryptopro.ru/products/csp. 

Далее необходимо скачать и установить КриптоПро .NET SDK.    https://www.cryptopro.ru/products/net/downloads

Здесь вся нужная документация и хорошо бы ее почитать: http://cpdn.cryptopro.ru/default.asp?url=content/cpnet/html/08bcd27a-1f1c-4494-a996-37d88776309e.htm 

В проекте нужно указать правильные ссылки в References. Скорее всего красными будут подчеркнуты:
- CryptoPro.Sharpei.Base
- CryptoPro.Sharpei.Xml
- Microsoft.CSharp
- System.Security
- Newtonsoft.Json
Первые 2 библиотеки устанавливаются с КриптоПро. Microsoft.CSharp и System.Security системные должны быть, Newtonsoft.Json лежит в проекте в папке Debug. 

Так же в проекте надо установить настройки в Properties.Settings:
- dictionaryDir
- apiHost
- signerName
- ogrn
- kpp
signerName - это название подписи, которой будете подписывать все запросы, apiHost - хост тестового сервера (http://85.142.162.12:8031), остальные в описании думаю не нуждаются


