using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.Amateras.Common.Utilities;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using Dapper;
using Microsoft.Extensions.Logging;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    public class InformationSecurityService : IInformationSecurityService
    {
        private readonly ILogger<InformationSecurityService> _logger;
        private readonly IInformationSecurityDBAccessor _informationSecurityDBAccessor;

        public InformationSecurityService(
            ILogger<InformationSecurityService> logger,
            IInformationSecurityDBAccessor informationSecurityDBAccessor)
        {

            _logger = logger;
            _informationSecurityDBAccessor = informationSecurityDBAccessor;
        }

        /// <summary>
        /// 会社体制情報一覧取得（Async）
        /// </summary>
        public async Task<List<InformationSecurityDto>> QueryInformationSecurityListAsync(
            string searchKeyword,
            CancellationToken ct = default)
        {
            _logger.LogDebug("┏DB接続Open");
            var resultRaw = await _informationSecurityDBAccessor.GetAllInformationSecurityRecordsByKeyword(searchKeyword);
            var dataDb =  resultRaw.AsList();
            var dataDto =  MapDBtoDTO(dataDb);
            return dataDto;
        }


        /// <summary>
        /// 情報セキュリティ検索
        /// </summary>
        public async Task<List<InformationSecurityDto>> QueryInformationSecuritySearchAsync(
            InformationSecuritySearchDto dto,
            CancellationToken ct = default)
        {
            _logger.LogDebug("┏DB接続Open");
            var resultRaw = await _informationSecurityDBAccessor.GetInformationSecurityRecordsBySearch(dto);
            var dataDb =  resultRaw.AsList();
            var dataDto =  MapDBtoDTO(dataDb);
            return dataDto;
        }


        private List<InformationSecurityDto> MapDBtoDTO(List<InformationSecurity> dataDb)
        {
            var dataDto =  dataDb.Select(x => new InformationSecurityDto(x)).ToList();

            if (dataDto.Count == 0)
            {   
                return dataDto;                
            }

            int mainRecordIndex = 0;
            int mainRecordChildrenNumber = 1;
            var mainObjList = new List<(int mainRecordIndex, int mainRecordChildrenNumber )>();

            for (int i = 1; i < dataDto.Count; i++)
            {
                if (dataDto[i].companyCode1 == dataDto[i-1].companyCode1 && dataDto[i].companyCode2 == dataDto[i-1].companyCode2)
                {
                    mainRecordChildrenNumber++;
                }
                else
                {
                    mainObjList.Add((mainRecordIndex, mainRecordChildrenNumber));
                    mainRecordIndex = i;
                    mainRecordChildrenNumber = 1;
                }
            }

            mainObjList.Add((mainRecordIndex, mainRecordChildrenNumber));

            for (int i = 0; i < mainObjList.Count; i++)
            {
                dataDto[mainObjList[i].mainRecordIndex].isMainRecord = true;
                dataDto[mainObjList[i].mainRecordIndex].childrenNumber = mainObjList[i].mainRecordChildrenNumber;
            }

            _logger.LogDebug($"会社体制情報一覧取得 {dataDto.Count}件取得");
            return dataDto;
        }
    }
}