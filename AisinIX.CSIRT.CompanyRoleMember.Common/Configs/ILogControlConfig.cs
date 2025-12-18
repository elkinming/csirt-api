
namespace AisinIX.CSIRT.CompanyRoleMember.Common.Configs
{
    /// <summary>
    /// ログサービスの構成設定
    /// </summary>
    public interface ICompanyRoleMemberConfig
    {
        /// <summary>
        /// ログサービスのURL
        /// </summary>
        string CompanyRoleMemberServiceUrl {get; }
    }
}