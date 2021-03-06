﻿using System;
using System.Collections.Generic;

using WarnerEngine.Lib;

namespace WarnerEngine.Services
{
    public interface IInteractionService : IService
    {
        DisposableList<T> GetCachedEntities<T>();
        IInteractionService RegisterAction(BaseInteraction Action);
    }
}
