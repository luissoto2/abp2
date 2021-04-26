using System;
using System.Threading.Tasks;

namespace Volo.Abp.DistributedLocking
{
    public interface IDistributedLockManager
    {
        /// <summary>
        /// Tries to lock a resource.
        /// </summary>
        /// <param name="resourceName">The resource name</param>
        /// <param name="expireTime">
        /// Expiration time for the lock.
        /// Keep this expire time small and use <see cref="RefreshLockAsync"/>
        /// if you need to continue to obtain the lock.
        /// </param>
        /// <returns>
        /// True, if lock is obtained.
        /// </returns>
        Task<bool> TryLockAsync(string resourceName, TimeSpan expireTime);
        
        /// <summary>
        /// Refreshes (extends) an already obtained lock.
        /// </summary>
        /// <param name="resourceName">The resource name</param>
        /// <param name="expireTime">
        /// Expiration time for the lock.
        /// Keep this expire time small and refresh again
        /// if you need to continue to obtain the lock.
        /// </param>
        Task RefreshLockAsync(string resourceName, TimeSpan expireTime);
        
        /// <summary>
        /// Unlocks a pre-locked resource.
        /// </summary>
        /// <param name="resourceName">The resource name</param>
        Task UnlockAsync(string resourceName);
    }
}