using System.Text;

public enum AccountType
{
	Checking,
	Savings,
	Deposit
}

public sealed class BankTransaction
{
	public DateTime Timestamp { get; }
	public decimal Amount { get; }
	public string? Description { get; }

	public BankTransaction(decimal amount, string? description = null)
	{
		Timestamp = DateTime.Now;
		Amount = amount;
		Description = description;
	}
}

public sealed class BankAccount : IEquatable<BankAccount>
{
	public string AccountNumber { get; }
	public AccountType AccountType { get; }
	public string Holder { get; set; }

	public string Currency { get; }
	public decimal Balance { get; private set; }

	private readonly List<BankTransaction> _transactions = new();

	public BankAccount(string accountNumber, AccountType accountType, string holder, string currency, decimal balance)
	{
		AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
		AccountType = accountType;
		Holder = holder ?? throw new ArgumentNullException(nameof(holder));
		Currency = currency ?? throw new ArgumentNullException(nameof(currency));
		Balance = balance;
	}

	public BankTransaction this[int index]
	{
		get => _transactions[index];
	}

	public int TransactionsCount => _transactions.Count;

	public void AddTransaction(BankTransaction transaction)
	{
		if (transaction is null) throw new ArgumentNullException(nameof(transaction));
		_transactions.Add(transaction);
		Balance += transaction.Amount;
	}

	public override string ToString()
	{
		return $"{Holder}, Account: {AccountNumber} ({AccountType}), Balance: {Balance} {Currency}";
	}

	public bool Equals(BankAccount? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;

		return string.Equals(AccountNumber, other.AccountNumber, StringComparison.Ordinal)
			&& string.Equals(Holder, other.Holder, StringComparison.Ordinal)
			&& string.Equals(Currency, other.Currency, StringComparison.Ordinal)
			&& AccountType == other.AccountType
			&& Balance == other.Balance;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as BankAccount);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(AccountNumber, Holder, Currency, AccountType, Balance);
	}

	public static bool operator ==(BankAccount? left, BankAccount? right)
	{
		if (left is null && right is null) return true;
		if (left is null || right is null) return false;
		return left.Equals(right);
	}

	public static bool operator !=(BankAccount? left, BankAccount? right)
	{
		return !(left == right);
	}
}

public sealed class Building
{
	public int Number { get; }
	public string? Address { get; }

	public Building(int number, string? address = null)
	{
		Number = number;
		Address = address;
	}

	public override string ToString()
	{
		return Address is null ? $"Building #{Number}" : $"Building #{Number}: {Address}";
	}
}

public sealed class Buildings
{
	private readonly Building?[] _items = new Building?[10];

	public Building? this[int number]
	{
		get
		{
			if (number < 0 || number >= _items.Length) throw new IndexOutOfRangeException();
			return _items[number];
		}
		set
		{
			if (number < 0 || number >= _items.Length) throw new IndexOutOfRangeException();
			_items[number] = value;
		}
	}

	public int Capacity => _items.Length;
}

internal static class Program
{
	private static void Main()
	{
		Console.OutputEncoding = Encoding.UTF8;

		var account = new BankAccount(
			accountNumber: "40817810099900000001",
			accountType: AccountType.Checking,
			holder: "Иван Петров",
			currency: "RUB",
			balance: 10000m);

		Console.WriteLine("=== Упражнение 10.1: BankAccount и BankTransaction (свойства) ===");
		Console.WriteLine($"Номер счёта (только чтение): {account.AccountNumber}");
		Console.WriteLine($"Тип счёта (только чтение): {account.AccountType}");
		Console.WriteLine($"Держатель (чтение/запись): {account.Holder}");
		Console.WriteLine($"Баланс: {account.Balance} {account.Currency}\n");

		account.Holder = "Иван Иванов";
		Console.WriteLine($"Новый держатель: {account.Holder}\n");

		account.AddTransaction(new BankTransaction(2500m, "Пополнение"));
		account.AddTransaction(new BankTransaction(-1200m, "Снятие наличных"));
		account.AddTransaction(new BankTransaction(500m, "Кэшбэк"));

		Console.WriteLine("После транзакций:");
		Console.WriteLine($"Баланс: {account.Balance} {account.Currency}");
		Console.WriteLine($"Количество операций: {account.TransactionsCount}\n");

		Console.WriteLine("=== Упражнение 10.2: Индексатор в BankAccount для транзакций ===");
		for (var i = 0; i < account.TransactionsCount; i++)
		{
			var t = account[i];
			Console.WriteLine($"[{i}] {t.Timestamp:yyyy-MM-dd HH:mm:ss} | {t.Amount,8:+0.00;-0.00} {account.Currency} | {t.Description}");
		}
		Console.WriteLine();

		Console.WriteLine("=== Упражнение 10.3: Коллекция Buildings с индексатором ===");
		var buildings = new Buildings();
		buildings[0] = new Building(0, "ул. Ленина, 10");
		buildings[1] = new Building(1, "пр-т Мира, 25");
		buildings[5] = new Building(5, "ул. Пушкина, 1");

		for (var n = 0; n < buildings.Capacity; n++)
		{
			var b = buildings[n];
			Console.WriteLine(b is null ? $"[{n}] пусто" : $"[{n}] {b}");
		}
	}
}
