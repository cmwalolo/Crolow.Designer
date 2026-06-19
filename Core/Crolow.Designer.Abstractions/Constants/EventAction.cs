namespace Crolow.Designer.Common.Constants;


public enum EventAction
{
    ObjectCreated = 1,
    ObjectUpdated = 2,
    ObjectDeleted = 4,
    ChildrenCreated = 8,
    ChildrenUpdated = 16,
    ChildrenDeleted = 32,
    ObjectActivated = 64,
    // => This will be used for a list that embeds multiple actions
    // For instance when you move an object from on parent to another
    //      => The action will proceed to delete/create in the parent nodes 
    //      => invalidate all involved parents
    //      => invalidate moved objects
    // Instead of publishing multiple actoins, we just publish one
    // The receiver can then updates his viewstate at once, and refresh the component.
    // See EventContainer class
    Mixed = 128
}