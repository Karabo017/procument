export type AuditLogAction = 'CREATE' | 'UPDATE' | 'DELETE' | 'PUBLISH' | 'AWARD';

export interface AuditLogEntry {
  id?: string;
  user?: string;
  action: AuditLogAction;
  message: string;
  timestamp?: Date | string;
  entity?: string;
  entityId?: string;
}

export class AuditLogs {
  items: AuditLogEntry[] = [];

  add(entry: Partial<AuditLogEntry>) {
    this.items.push({
      id: entry.id ?? genId(),
      user: entry.user ?? 'system',
      action: entry.action ?? 'UPDATE',
      message: entry.message ?? '',
      timestamp: entry.timestamp ?? new Date().toISOString(),
      entity: entry.entity,
      entityId: entry.entityId,
    });
  }

  clear() {
    this.items = [];
  }
}

export default AuditLogs;

function genId() {
  return Math.random().toString(36).slice(2) + Date.now().toString(36);
}