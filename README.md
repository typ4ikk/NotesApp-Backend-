# NotesApp-Backend-
Серверная часть приложения для заметок.
Модели: заметки (Note), тэги (Tag), модель для связи заметок и тэгов многие к многим (NoteTag)
Функционал: создание, редактирование, удаление заметок; создание и установка тегов; прикрепление заметки к определенной дате и определенному времени; список заметок и напоминаний.
Запуск приложения: через команду dotnet run в терминале среды разработки или в Командной строке. ВАЖНО! Перед запуском настройте подключение к вашей базе данных PostgreSQL в appsetings.json "DefaultConnection" и "MasterConnection"
Для отправки запросов на сервер воспользуйтесь Postman или другим схожим приложением
