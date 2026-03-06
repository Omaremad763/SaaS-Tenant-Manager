import { NavItem } from '../shared_models/nav-item.model';

export const MENU_ITEMS: NavItem[] = [
  {
    label: 'My Subscription',
    icon: 'credit-card',
    route: '/dashboard',
    roles: ['TenantAdmin', 'SystemAdmin'],
  },
  {
    label: 'Admin Panel',
    icon: 'shield-halved',
    route: '/AdminPanel',
    roles: ['SystemAdmin'],
  },
  {
    label: 'Shipments',
    icon: 'truck',
    route: '/Shipments',
    roles: ['TenantAdmin', 'TenantUser'],
  },
  {
    label: 'Clients',
    icon: 'user',
    route: '/Clients',
    roles: ['TenantAdmin', 'TenantUser'],
  },
  {
    label: 'Shipment Wizard',
    icon: 'wand-magic-sparkles',
    route: '/shipmentwizard',
    roles: ['TenantAdmin', 'TenantUser'],
  },
];
