using System;
using System.Collections.Generic;
namespace AisinIX.CSIRT.CompanyRoleMember.Common.Models
{
    /// <summary>
    /// 非同期処理の前後処理のインターフェースです。
    /// </summary>
    public interface IAsyncProcessHelper
    {
        /// <summary>
        /// 非同期処理の前処理を行います。
        /// </summary>
        void PreInvoke();

        /// <summary>
        /// 非同期処理の後処理を行います。
        /// </summary>
        void PostInvoke();
    }
}