using Domain.Common;

namespace Application.Filters;

public  record CarFilter(
    string? Brand = null,           // Фильтрация по бренду
    string? Model = null,           // Фильтрация по модели
    int? YearFrom = null,           // Фильтрация по минимальному году
    int? YearTo = null,             // Фильтрация по максимальному году
    string? Category = null,        // Фильтрация по категории
    string? Location = null,        // Фильтрация по местоположению
    string? RegistrationNumber = null // Фильтрация по регистрационному номеру
) : BaseFilter;