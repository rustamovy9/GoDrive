using Domain.Common;

namespace Application.Filters;

public  record CarFilter(
    string? Brand = null,           // Фильтрация по бренду
    string? Model = null,           // Фильтрация по модели
    int? YearFrom = null,           // Фильтрация по минимальному году
    int? YearTo = null,             // Фильтрация по максимальному году
    int? CategoryId = null,        // Фильтрация по категории
    int? LocationId = null,        // Фильтрация по местоположению
    string? RegistrationNumber = null // Фильтрация по регистрационному номеру
) : BaseFilter;