using DatabaseAPI.Factories;
using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;
using DatabaseAPI.TableItemSectionPlacement;

namespace DatabaseAPI
{
    public interface IDatabaseService
    {
        ITableItem TableItem { get; }

        ITableItemGroup TableItemGroup { get; }

        ITableFloorplan TableFloorplan { get; }

        ITableStoreSection TableStoreSection { get; }
        ITableItemSectionPlacement TableItemSectionPlacement { get; }
    }
}