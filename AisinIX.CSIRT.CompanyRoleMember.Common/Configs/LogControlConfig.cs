using AisinIX.Amateras.Common.Utilities;

namespace AisinIX.CSIRT.CompanyRoleMember.Common.Configs
{
    /// <summary>
    /// ログサービスの構成設定サービスの構成設定
    /// </summary>
    public class CompanyRoleMemberConfig : ICompanyRoleMemberConfig
    {
        private readonly IConfigUtility _ConfigUtility;

        /// <summary>
        /// ログサービスのURL
        /// </summary>
        public string CompanyRoleMemberServiceUrl { get; private set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CompanyRoleMemberConfig(IConfigUtility ConfigUtility)
        {
            _ConfigUtility = ConfigUtility;
            CompanyRoleMemberServiceUrl = _ConfigUtility.GetStringSetting("CompanyRoleMember.Service.URL", string.Empty);
        }
    }
}