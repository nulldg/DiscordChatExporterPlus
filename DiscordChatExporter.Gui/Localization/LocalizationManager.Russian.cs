using System.Collections.Generic;

namespace DiscordChatExporter.Gui.Localization;

public partial class LocalizationManager
{
    private static readonly IReadOnlyDictionary<string, string> RussianLocalization =
        new Dictionary<string, string>
        {
            // Dashboard
            [nameof(PullGuildsTooltip)] = "Загрузить доступные серверы и каналы (Enter)",
            [nameof(SettingsTooltip)] = "Настройки",
            [nameof(LastMessageSentTooltip)] = "Последнее сообщение:",
            [nameof(TokenPlaceholderText)] = "Токен",
            // Token instructions (personal account)
            [nameof(TokenPersonalHeader)] = "Как получить токен для персонального аккаунта:",
            [nameof(TokenPersonalTosWarning)] =
                "* Автоматизация учетных записей технически нарушает Условия обслуживания — **на свой страх и риск**!",
            [nameof(TokenPersonalInstructions)] = """
                1. Откройте Discord в вашем веб-браузере и войдите
                2. Откройте любой сервер или канал личных сообщений
                3. Нажмите **Ctrl+Shift+I**, чтобы открыть инструменты разработчика
                4. Перейдите на вкладку **Network**
                5. Нажмите **Ctrl+R** для перезагрузки
                6. Переключайтесь между каналами, чтобы вызвать сетевые запросы
                7. Найдите запрос, который начинается с **messages**
                8. Выберите вкладку **Headers** справа
                9. Прокрутите до раздела **Request Headers**
                10. Скопируйте значение заголовка **authorization**
                """,
            // Token instructions (bot)
            [nameof(TokenBotHeader)] = "Как получить токен для бота:",
            [nameof(TokenBotInstructions)] = """
                Токен генерируется при создании бота. Если вы его потеряли, сгенерируйте новый:

                1. Откройте Discord [портал разработчика](https://discord.com/developers/applications)
                2. Откройте настройки вашего приложения
                3. Перейдите в раздел **Bot** слева
                4. В разделе **Token** нажмите **Reset Token**
                5. Нажмите **Yes, do it!** и подтвердите
                * Интеграции, использующие предыдущий токен, перестанут работать
                * У вашего бота должен быть включен **Message Content Intent** для чтения сообщений
                """,
            [nameof(TokenHelpText)] =
                "Если у вас есть вопросы или проблемы, обратитесь к [документации](https://github.com/nulldg/DiscordChatExporterPlus/tree/master/.docs)",
            // Settings
            [nameof(SettingsTitle)] = "Настройки",
            [nameof(ThemeLabel)] = "Тема",
            [nameof(ThemeTooltip)] = "Предпочитаемая тема интерфейса",
            [nameof(LanguageLabel)] = "Язык",
            [nameof(LanguageTooltip)] = "Предпочитаемый язык интерфейса",
            [nameof(AutoUpdateLabel)] = "Автообновление",
            [nameof(AutoUpdateTooltip)] = "Выполнять автоматические обновления при каждом запуске",
            [nameof(PersistTokenLabel)] = "Сохранять токен",
            [nameof(PersistTokenTooltip)] = """
                Сохранять последний использованный токен в файле для сохранения между сеансами.
                **Внимание**: хотя токен сохраняется с шифрованием, он может быть восстановлен злоумышленником, имеющим доступ к вашей системе.
                """,
            [nameof(RateLimitPreferenceLabel)] = "Лимит запросов",
            [nameof(RateLimitPreferenceTooltip)] =
                "Соблюдать ли рекомендованные лимиты запросов. Если отключено, будут соблюдаться только жесткие лимиты (т.е. ответы 429).",
            [nameof(ShowThreadsLabel)] = "Показывать ветки",
            [nameof(ShowThreadsTooltip)] = "Какие типы веток показывать в списке каналов",
            [nameof(LocaleLabel)] = "Локаль",
            [nameof(LocaleTooltip)] = "Локаль для форматирования дат и чисел",
            [nameof(NormalizeToUtcLabel)] = "Нормализовать до UTC",
            [nameof(NormalizeToUtcTooltip)] = "Нормализовать все временные метки до UTC+0",
            [nameof(ParallelLimitLabel)] = "Лимит параллелизации",
            [nameof(ParallelLimitTooltip)] = "Сколько каналов может экспортироваться одновременно",
            // Export Setup
            [nameof(ChannelsSelectedText)] = "каналов выбрано",
            [nameof(OutputPathLabel)] = "Путь сохранения",
            [nameof(OutputPathTooltip)] = """
                Путь к файлу или директории вывода.

                Если указана директория, имена файлов будут генерироваться автоматически на основе названий каналов и параметров экспорта.

                Пути к директориям должны заканчиваться слешем во избежание неоднозначности.

                Доступные шаблонные токены:
                **%g** — ID сервера
                **%G** — название сервера
                **%t** — ID категории
                **%T** — название категории
                **%c** — ID канала
                **%C** — название канала
                **%p** — позиция канала
                **%P** — позиция категории
                **%a** — дата после
                **%b** — дата до
                **%d** — текущая дата
                """,
            [nameof(FormatLabel)] = "Формат",
            [nameof(FormatTooltip)] = "Формат экспорта",
            [nameof(AfterDateLabel)] = "После (дата)",
            [nameof(AfterDateTooltip)] = "Включать только сообщения, отправленные после этой даты",
            [nameof(BeforeDateLabel)] = "До (дата)",
            [nameof(BeforeDateTooltip)] = "Включать только сообщения, отправленные до этой даты",
            [nameof(AfterTimeLabel)] = "После (время)",
            [nameof(AfterTimeTooltip)] = "Включать только сообщения, отправленные после этого времени",
            [nameof(BeforeTimeLabel)] = "До (время)",
            [nameof(BeforeTimeTooltip)] = "Включать только сообщения, отправленные до этого времени",
            [nameof(PartitionLimitLabel)] = "Разделять экспорт",
            [nameof(PartitionLimitTooltip)] =
                "Разделить вывод на части, каждая ограничена указанным количеством сообщений (напр. '100') или размером файла (напр. '10mb')",
            [nameof(MessageFilterLabel)] = "Фильтр сообщений",
            [nameof(MessageFilterTooltip)] =
                "Включать только сообщения, соответствующие этому фильтру (напр. 'from:foo#1234' или 'has:image'). Смотрите документацию для более подробной информации.",
            [nameof(ReverseMessageOrderLabel)] = "Обратный порядок сообщений",
            [nameof(ReverseMessageOrderTooltip)] =
                "Экспортировать сообщения в обратном хронологическом порядке (сначала новые)",
            [nameof(FormatMarkdownLabel)] = "Форматировать markdown",
            [nameof(FormatMarkdownTooltip)] =
                "Обрабатывать markdown, упоминания и другие специальные токены",
            [nameof(DownloadAssetsLabel)] = "Загружать ресурсы",
            [nameof(DownloadAssetsTooltip)] =
                "Загружать ресурсы, на которые ссылается экспорт (аватары, вложенные файлы, встроенные изображения и т.д.)",
            [nameof(ReuseAssetsLabel)] = "Повторно использовать ресурсы",
            [nameof(ReuseAssetsTooltip)] =
                "Повторно использовать ранее загруженные ресурсы, чтобы избежать лишних запросов",
            [nameof(AssetsDirPathLabel)] = "Путь к директории ресурсов",
            [nameof(AssetsDirPathTooltip)] =
                "Загружать ресурсы в эту директорию. Если не указано, путь к директории ресурсов будет определен из пути сохранения.",
            [nameof(AdvancedOptionsTooltip)] = "Переключить расширенные параметры",
            [nameof(ExportButton)] = "ЭКСПОРТИРОВАТЬ",
            // Common buttons
            [nameof(CloseButton)] = "ЗАКРЫТЬ",
            [nameof(CancelButton)] = "ОТМЕНИТЬ",
            // Dialog messages
            [nameof(UnstableBuildTitle)] = "Предупреждение о нестабильной сборке",
            [nameof(UnstableBuildMessage)] = """
                Вы используете сборку разработки {0}. Эти сборки не прошли тщательного тестирования и могут содержать ошибки.

                Автообновление отключено для сборок разработки.

                Нажмите ПОСМОТРЕТЬ РЕЛИЗЫ, чтобы загрузить стабильный релиз.
                """,
            [nameof(SeeReleasesButton)] = "ПОСМОТРЕТЬ РЕЛИЗЫ",
            [nameof(UpdateDownloadingMessage)] = "Загрузка обновления {0} v{1}...",
            [nameof(UpdateReadyMessage)] = "Обновление загружено и будет установлено после выхода",
            [nameof(UpdateInstallNowButton)] = "УСТАНОВИТЬ СЕЙЧАС",
            [nameof(UpdateFailedMessage)] = "Не удалось выполнить обновление программы",
            [nameof(ErrorPullingGuildsTitle)] = "Ошибка загрузки серверов",
            [nameof(ErrorPullingChannelsTitle)] = "Ошибка загрузки каналов",
            [nameof(ErrorExportingTitle)] = "Ошибка экспорта канала(-ов)",
            [nameof(SuccessfulExportMessage)] = "Успешно экспортировано {0} канал(-ов)",
        };
}