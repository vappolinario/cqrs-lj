public record OrderDto
(
  int iD,
  string FirstName,
  string LastName,
  string Status,
  DateTime CreatedAt,
  Decimal TotalCost
);
