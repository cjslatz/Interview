import { Counter } from "./components/Counter";
import DocumentManager from "./components/DocumentManager";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
        element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <DocumentManager />
  },
  {
    path: '/document-manager',
    element: <DocumentManager />
  }
];

export default AppRoutes;
