using System.Reflection;

namespace CleanArch.Domain.SeedWork;

/// <summary>
/// 參考 "使用列舉類別，而非列舉類型"
/// https://learn.microsoft.com/zh-tw/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
/// </summary>
public abstract class Enumeration : IComparable
{
    public int Id { get; private set; }
    public string? Name { get; private set; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public override string ToString() => Name!;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();


    /// <summary>
    /// 檢查Enum 是否為同一個
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue) return false;
        return GetType().Equals(obj.GetType()) && Id.Equals(otherValue.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        => Math.Abs(firstValue.Id - secondValue.Id);

    public static T FromValue<T>(int value) where T : Enumeration
        => Parse<T, int>(value, "value", item => item.Id == value);

    public static T FromDisplayName<T>(string displayName) where T : Enumeration
        => Parse<T, string>(displayName, "display name", item => item.Name == displayName);

    /// <summary>
    /// 傳入判斷式取得enum
    /// </summary>
    /// <param name="value">判斷值(log)</param>
    /// <param name="description">描述(log)</param>
    /// <param name="predicate">判斷式</param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <returns></returns>
    protected static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);
        if (matchingItem == null) throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        return matchingItem;
    }

    public int CompareTo(object? other) => Id.CompareTo((other as Enumeration)!.Id);
}
