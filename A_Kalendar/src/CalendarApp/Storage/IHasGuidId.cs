using System;

namespace CalendarApp.Storage;

public interface IHasGuidId
{
    Guid Id { get; set; }
}

