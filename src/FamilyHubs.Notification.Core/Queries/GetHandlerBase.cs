using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.Repository;
using FamilyHubs.Notification.Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Notification.Core.Queries;

public class GetHandlerBase
{
    protected readonly ApplicationDbContext _context;
    protected readonly IMapper _mapper;
    protected GetHandlerBase(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    protected async Task<PaginatedList<MessageDto>> GetPaginatedList(bool requestIsNull, IQueryable<SentNotification> referralList, int pageNumber, int pageSize)
    {
        int totalRecords = referralList.Count();
        List<MessageDto> pagelist;
        if (!requestIsNull)
        {
            pagelist = await referralList.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new PaginatedList<MessageDto>(pagelist, totalRecords, pageNumber, pageSize);
        }

        pagelist = _mapper.Map<List<MessageDto>>(referralList);
        var result = new PaginatedList<MessageDto>(pagelist.ToList(), totalRecords, 1, 10);
        return result;
    }

    protected IQueryable<SentNotification> OrderBy(IQueryable<SentNotification> currentList, NotificationOrderBy? orderBy, bool? isAssending)
    {
        if (orderBy == null || isAssending == null)
            return currentList;

        switch (orderBy)
        {
            case NotificationOrderBy.RecipientEmail:
                if (isAssending.Value)
                    return currentList.OrderBy(x => x.RecipientEmail);
                return currentList.OrderByDescending(x => x.RecipientEmail);

            case NotificationOrderBy.Created:
                if (isAssending.Value)
                    return currentList.OrderBy(x => x.Created);
                return currentList.OrderByDescending(x => x.Created);

            case NotificationOrderBy.TemplateId:
                if (isAssending.Value)
                    return currentList.OrderBy(x => x.TemplateId);
                return currentList.OrderByDescending(x => x.TemplateId);

        }

        return currentList;
    }
}
