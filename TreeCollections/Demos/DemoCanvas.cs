namespace TreeCollections.DemoConsole
{
    public static class DemoCanvas
    {
        public static void Start()
        {
            Demos.DemoSimpleMutableTree.Start();
            Demos.DemoSpecialCaseMoveOperation.Start();
            Demos.DemoSerialTree.Start();

            Demos.DemoReadOnlyTree.Start();
            Demos.DemoDefaultEntityTrees.Start();

            Demos.DemoSiblingRepositioning.Start();

            Demos.DemoPreOrderEnumeration.Start();

            Demos.DemoCopyTo.Start();
        }
    }
}
