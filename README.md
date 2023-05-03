# ClientRequestHandler
Обработка таблиц клиентов и заявок с использованием бд(MySQL)

В качестве базы данных использовалась MySql. Из сторонних библиотек использовалась только библиотека "MySql" для работы с БД, для всего остального использовались только встроенные в платформу решения.
Вся работа непосредственно с БД и с библиотекой "MySql" вынесена в статический класс DBWorker, все остальные классы обмениваются данными с БД через него.

В главном окне можно редактировать только статус заявок, который можно редактировать из любого места программы, где отображаются заявки.
Помимо главного окна есть еще 4 второстепенных окна:
1. Для добавления новых клиентов
2. Для добавления новых заявок
3. Для редактирования и удаления уже существующих клиентов
4. Для удаления уже существующих заявок

В главном окне и в 3м и 4м окнах таблицы синхронизированы друг с другом и с БД, все изменения в любой из них сразу же отображаются в других окнах и в БД. 
Добавление новых клиентов и заявок происходит в отдельных окнах(1м и 2м). Удаление и создание новых записей также синхронизируется со всеми таблицами и БД.

В качестве моделей в программе реализовано 2 класса:
Класс Client со свойствами:
1. Id 
2. Name
2. INN 
3. ActivityField
4. RequestCount
5. LastRequestDate
6. Note
Класс Request со свойствами:
1. Id
2. ClientId 
3. StartDate
4. Name
5. Description
6. Status 
В качестве ключей в обоих таблицах используются отдельные поля Id, следовательно, от остальных полей не требуется уникальность. Тем не менее, при  попытке добавить клиента с неуникальным именем или изменить имя клиента на неуникальное имя - возникает диалоговое окно с предупреждением.
Свойства RequestCount и LastRequestDate нельзя редактировать в программе, они задаются дефолтными значениями и обновляются с помощью триггеров в самой базе данных.

Создание таблиц и триггеров в БД, а также генерация начальных данных происходит автоматически во время старта работы программы, в конструкторе класса MainWindowViewModel.

При создании приложения использовалаь архитектура MVVM, поэтому вся логика реализована в классах -ViewModel - наследниках базового класса ViewModel, реализующего интерфейс INotifyPropertyChanged. 
Все нажатия на кнопки и комбинации клавиш реализованы с помощью команд - экземпляров класса RelayCommand, реализующего интерфейс ICommand.
