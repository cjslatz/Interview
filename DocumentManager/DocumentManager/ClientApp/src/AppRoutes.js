import DocumentManager from "./components/DocumentManager";

const AppRoutes = [
  {
    index: true,
        element: <DocumentManager />
  },
  {
    path: '/document-manager',
    element: <DocumentManager />
  }
];

export default AppRoutes;
