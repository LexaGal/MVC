﻿using System;
using PostSharp.Aspects;

namespace Aop.AopAspects.Caching
{
    [Serializable]
    public class CacheableResultAspect : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var cache = MethodResultCache.GetCache(args.Method);
            var result = cache.GetCachedResult(args.Arguments);
            if (result != null)
            {
                args.ReturnValue = result;
                return;
            }
            base.OnInvoke(args);
            cache.CacheCallResult(args.ReturnValue, args.Arguments);
        }
    }
}