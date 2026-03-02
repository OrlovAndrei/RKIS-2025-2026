using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.EfRepository;


public static class SimpleQueryableExtensions
{

    // Метод фильтрует все свойства которые содержат текст 
    public static IQueryable<T> WhereAnyPropertyContainText<T>(
        this IQueryable<T> source,
        string searchText,
        params Expression<Func<T, string>>[] properties)
    {
        //Это параметр, который будет использоваться во всех лямбдах массива properties, например x=>x.Customer.FirstName
        //Имя параметра x
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        //Достаем метод Contains для вызова поиска текста
        MethodInfo? containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])
            ?? throw new InvalidOperationException("Contains method not found.");
        Expression body = Expression.Constant(false);
        //Проходим по всему массиву лямбд properties, и для каждого выражения
        //проверяем содержание текста searchText через вызов метода Contains.
        //Если выражение(свойство) содержит текст, то добавляем выражение к результирующему дереву выражений 
        foreach (Expression<Func<T, string>> property in properties)
        {
            // Заменяем параметр исходного выражения на наш параметр
            // если имя параметр в лямбде не x, то меняем лямбду с o => o.ProductName на x => x.ProductName
            // Имя параметра x задано выше в переменной parameter.
            // Если не менять имена параметров в лямбдах, и они буду разные, то будет ошибка
            Expression propertyAccess = ReplaceAnotherParameter(property.Body, property.Parameters[0], parameter);
            MethodCallExpression containsCall = Expression.Call(propertyAccess, containsMethod, Expression.Constant(searchText));
            body = Expression.OrElse(body, containsCall);
        }

        //Возвращаем результирующую лямбду
        //На дебаге можно поставить точку останова и посмотреть ее
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        return source.Where(lambda);
    }

    //Метод фильтрует по свойству типа DateTime, и выбирает все сущности во временном диапазоне
    public static IQueryable<T> WhereDateTimeBetween<T>(this IQueryable<T> source,
        Expression<Func<T, DateTime>> dateSelector,
        DateTime startDate, DateTime endDate)
    {
        //Параметер используемый в лямбде
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

        //Преобразуем делегат в аргумент(результирующую дату)
        InvocationExpression dateAccess = Expression.Invoke(dateSelector, parameter);

        //Сравниваем что дата больше startDate
        BinaryExpression lowerBound = Expression.GreaterThanOrEqual(dateAccess, Expression.Constant(startDate));
        //Сравниваем что дата меньше endDate
        BinaryExpression upperBound = Expression.LessThanOrEqual(dateAccess, Expression.Constant(endDate));

        //Проводим операцию AND, если оба предыдущих выражения верны возвращаем True
        BinaryExpression body = Expression.AndAlso(lowerBound, upperBound);

        //Возвращаем результирующую лямбду
        //На дебаге можно поставить точку останова и посмотреть ее
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return source.Where(lambda);
    }

    //Метод фильтрует сущности по массиву лямбд predicates.
    //Если все лямбды возвращают true, то сущность добавляется в набор
    public static IQueryable<T> WhereAndConditions<T>(this IQueryable<T> source,
        params Expression<Func<T, bool>>[] predicates)
    {
        return source.WhereConditions(ConditionType.And, predicates);
    }

    //Метод фильтрует сущности по массиву лямбд predicates.
    //Если одна из лямбд возвращают true, то сущность добавляется в набор
    public static IQueryable<T> WhereOrConditions<T>(this IQueryable<T> source,
        params Expression<Func<T, bool>>[] predicates)
    {
        return source.WhereConditions(ConditionType.Or, predicates);
    }

    //Метод фильтрует сущности по массиву лямбд predicates.
    private static IQueryable<T> WhereConditions<T>(this IQueryable<T> source, ConditionType conditionType, params Expression<Func<T, bool>>[] predicates)
    {
        if (predicates.Length == 0)
            return source;

        if (predicates.Length == 1)
            return source.Where(predicates[0]);

        //Параметр используемый в лямбдах
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

        // Заменяем параметр исходного выражения на наш параметр
        // если имя параметр в лямбде не x, то меняем лямбду с o => o.ProductName на x => x.ProductName
        // Имя параметра x задано выше в переменной parameter.
        // Если не менять имена параметров в лямбдах, и они буду разные, то будет ошибка
        List<Expression<Func<T, bool>>> replacedPredicates = predicates.Select(p =>
        {
            var newBody = ReplaceAnotherParameter(p.Body, p.Parameters[0], parameter);
            return Expression.Lambda<Func<T, bool>>(newBody, parameter);
        }).ToList();

        Expression body = replacedPredicates[0].Body;
        //Проходим по всему массиву лямбд replacedPredicates, и для каждого выражения
        //Строим новое дерево выражений body, в зависимости от условия добавляем сравнение AND или OR
        for (int i = 1; i < replacedPredicates.Count; i++)
        {
            body = conditionType switch
            {
                ConditionType.And => Expression.AndAlso(body, replacedPredicates[i].Body),
                ConditionType.Or => Expression.OrElse(body, replacedPredicates[i].Body),
                _ => throw new ArgumentOutOfRangeException(nameof(conditionType), conditionType, null)
            };
        }

        //Возвращаем результирующую лямбду
        //На дебаге можно поставить точку останова и посмотреть ее
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return source.Where(lambda);
    }

    //Перечисление для изменения типа операции
    private enum ConditionType
    {
        And,
        Or
    }

    //Вспомогательный класс для замены старого параметра на новый
    private static Expression ReplaceAnotherParameter(
        Expression expression,
        ParameterExpression oldParameter,
        ParameterExpression newParameter)
    {
        ParameterReplacerVisitor visitor = new(oldParameter, newParameter);
        return visitor.Visit(expression);
    }

    //Класс наследуется от ExpressionVisitor
    //ExpressionVisitor - это специальный класс, который позволяет пройти по всем узлам
    //дерева выражений (ExpressionTree) и модифицировать их так как нам нужно.
    private class ParameterReplacerVisitor(
        ParameterExpression oldParameter,
        ParameterExpression newParameter) : ExpressionVisitor
    {
        //Переопределяем базовый метод VisitParameter, и заменяем старый параметр в лямбде на новый
        // то есть, если старый параметр x а новый y, то меняем лямбда поменяется с x=>x+1 на y=>y+1
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == oldParameter ? newParameter : base.VisitParameter(node);
        }
    }
}
