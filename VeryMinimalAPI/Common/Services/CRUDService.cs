using Microsoft.EntityFrameworkCore;
using VeryMinimalAPI.Data;
using VeryMinimalAPI.Data.Types;

namespace VeryMinimalAPI.Common.Services;

public class CrudService<T>(AppDbContext db)
    where T : Entity
{
    public virtual async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return await db.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (Exception)
        {
            return [];
        }
    }

    public virtual async Task<T?> Get(int id, CancellationToken cancellationToken)
    {
        try
        {
            var data = await db.Set<T>().AsNoTracking().FirstAsync(x => x.Id == id, cancellationToken);
            return data;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public virtual async Task<IEnumerable<string>> Create(T data, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await db.Set<T>().AddAsync(data, cancellationToken);
            
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return [];
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return [e.Message];
        }
    }
    
    public virtual async Task<IEnumerable<string>> CreateBatch(IEnumerable<T> data, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await db.Set<T>().AddRangeAsync(data, cancellationToken);
            
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return [];
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return [e.Message];
        }
    }
    
    public virtual async Task<IEnumerable<string>> Update(T inputData, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (await db.Set<T>().AnyAsync(x => inputData.Equals(x.Id), cancellationToken))
                throw new Exception($"Data is not found based on the provided id");

            db.Set<T>().Update(inputData);
            
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return [];
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return [e.Message];
        }
    }
    
    public virtual async Task<IEnumerable<string>> UpdateBatch(IEnumerable<T> inputDatas, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            inputDatas = inputDatas.ToList();
            
            var ids = inputDatas.Select(x => x.Id);
            if (!await db.Set<T>().AllAsync(x => ids.Contains(x.Id), cancellationToken))
                throw new Exception($"Data is not found based on the provided id");

            foreach (var inputData in inputDatas)
            {
                db.Set<T>().Update(inputData);
            }
            
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return [];
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return [e.Message];
        }
    }
    
    public virtual async Task<IEnumerable<string>> Delete(int id, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var data = await db.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (data is null)
                throw new Exception($"Data is not found based on the provided id");

            db.Set<T>().Remove(data);
            
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return [];
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return [e.Message];
        }
    }
    
    public virtual async Task<IEnumerable<string>> DeleteBatch(IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {   
            if (!await db.Set<T>().AllAsync(x => ids.Contains(x.Id), cancellationToken))
                throw new Exception($"Data is not found based on the provided id");

            await db.Set<T>().Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken);
            
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return [];
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return [e.Message];
        }
    }
}