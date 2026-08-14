namespace zadanie_kamsoft.DTOs;

public record ParseResponce
(
    bool Success,
    int Processed_count,
    Object? Data,
    String ErrorMessege = null
);