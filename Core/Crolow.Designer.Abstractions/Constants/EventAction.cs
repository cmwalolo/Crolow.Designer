namespace Crolow.Designer.Common.Constants;

public enum EventAction
{
    ObjectCreated = 1,
    ObjectUpdated = 2,
    ObjectDeleted = 4,
    ChildrenCreated = 8,
    ChildrenUpdated = 16,
    ChildrenDeleted = 32,
    ObjectActivated = 64
}