import { NavItem } from '../shared_models/nav-item.model';

export const MENU_ITEMS: NavItem[] = [
  {
    label: 'My Subscription',
    icon: 'credit-card',
    route: '/subscription',
    roles: ['TenantAdmin', 'SystemAdmin'],
  },

  {
    label: 'Tenant Operations',
    icon: 'briefcase',
    route: '/operations',
    roles: ['TenantAdmin', 'TenantUser'],
  },

  {
    label: 'Admin Panel',
    icon: 'shield-halved',
    route: '/AdminPanel',
    roles: ['SystemAdmin'],
  },
];
