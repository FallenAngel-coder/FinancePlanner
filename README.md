# Finance Planner — Система обліку особистих фінансів

Цей проект є інструментом для планування та контролю особистих фінансів, що дозволяє відстежувати доходи, витрати, планувати бюджети та аналізувати фінансовий стан за допомогою різноманітних звітів.

## 🚀 Функціональність проекту

### 1. Інтерфейс користувача (UI)
*   **Система авторизації та реєстрації**: Безпечний вхід та створення облікових записів для ізоляції даних різних користувачів.
    *   📄 Реалізація: [AuthForm.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Forms/AuthForm.cs)
*   **Головний екран (Dashboard)**: Центральна панель з історією транзакцій за обрану дату та порівнянням прогнозів з реальними витратами.
    *   📄 Реалізація: [MainMenuDesigner.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/MainMenuDesigner.cs)
*   **Управління транзакціями**: Додавання, редагування та видалення записів про доходи та витрати з можливістю детального опису.
    *   📄 Реалізація: [AddTransactionForm.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Forms/AddTransactionForm.cs)
*   **Конструктор категорій**: Створення власних категорій витрат/доходів з визначенням їхнього життєвого циклу (постійні або обмежені часом).
    *   📄 Реалізація: [AddCategoryForm.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Forms/AddCategoryForm.cs)
*   **Модуль аналітики**: Візуальне представлення фінансових результатів за довільний період з потужною системою фільтрації.
    *   📄 Реалізація: [AnalyticsForm.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Forms/AnalyticsForm.cs)

### 2. Бізнес-логіка
*   **Гнучкий розрахунок балансу**: Підтримка різних алгоритмів обчислення фінансового стану (загальний баланс, місячний баланс, сума тільки витрат) через патерн *Strategy*.
    *   📂 Логіка: [Strategies/](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Services/Strategies/)
*   **Інтелектуальне управління періодами категорій**: Логіка, що дозволяє перейменовувати або видаляти категорії тільки для конкретного місяця, автоматично створюючи копії для збереження цілісності історії.
    *   📄 Реалізація: [CategoryService.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Services/CategoryService.cs)
*   **Фабрика об'єктів**: Стандартизоване створення транзакцій через централізовану фабрику.
    *   📄 Реалізація: [TransactionFactory.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Factories/TransactionFactory.cs)

### 3. Збереження даних
*   **База даних SQLite**: Надійне локальне збереження всіх транзакцій, категорій та даних користувачів.
    *   📄 Схема та ініціалізація: [DatabaseInitializer.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Data/DatabaseInitializer.cs)
*   **Ізоляція даних користувача**: Кожен запит до БД автоматично фільтрується за `UserId` поточного сеансу.
    *   📄 Реалізація репозиторію: [TransactionRepository.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Repositories/TransactionRepository.cs)
*   **Безпека**: Використання хешування SHA-256 для захисту паролів користувачів.
    *   📄 Логіка безпеки: [UserRepository.cs](file:///c:/Users/vahnu/OneDrive/%D0%A0%D0%BE%D0%B1%D0%BE%D1%87%D0%B8%D0%B9%20%D1%81%D1%82%D1%96%D0%BB/%D0%9F%D0%BE%D0%BB%D1%96%D1%82%D0%B5%D1%85/Visual%20KPZ/FinancePlanner/FinancePlanner/Repositories/UserRepository.cs)

## 🛠 Технологічний стек
*   **Мова**: C# (.NET)
*   **UI**: Windows Forms
*   **БД**: SQLite (Microsoft.Data.Sqlite)
*   **Архітектура**: Layered Architecture (Models, Repositories, Services, Forms)

## 🧩 Використані патерни проектування
1.  **Singleton**: Управління підключенням до БД та сесією користувача.
2.  **Repository**: Ізоляція логіки доступу до даних.
3.  **Strategy**: Динамічна зміна алгоритмів розрахунку балансу.
4.  **Factory**: Централізоване створення об'єктів моделей.
5.  **Service Layer**: Відокремлення складної бізнес-логіки від інтерфейсу.
