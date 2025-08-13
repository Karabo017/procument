// Lightweight stub to satisfy legacy imports during CI builds
// Remove when Angular app is fully retired.

export type AuditLogAction = 'CREATE' | 'UPDATE' | 'DELETE' | 'PUBLISH' | 'AWARD';

export interface AuditLogEntry {
  id?: string;
  user?: string;
  action: AuditLogAction;
  message: string;
  timestamp: Date;
  entity?: string;
  entityId?: string;
}

export class AuditLogs {
  private _items: AuditLogEntry[] = [];

  get items(): AuditLogEntry[] {
    return this._items;
  }

  add(entry: Partial<AuditLogEntry>) {
    const e: AuditLogEntry = {
      id: entry.id ?? cryptoRandom(),
      user: entry.user ?? 'system',
      action: entry.action ?? 'UPDATE',
      message: entry.message ?? '',
      timestamp: entry.timestamp ?? new Date(),
      entity: entry.entity,
      entityId: entry.entityId
    };
    this._items.push(e);
  }

  clear() {
    this._items = [];
  }

  filter(predicate: (e: AuditLogEntry) => boolean): AuditLogEntry[] {
    return this._items.filter(predicate);
  }

  static from(raw: any[]): AuditLogs {
    const logs = new AuditLogs();
    (raw ?? []).forEach(r =>
      logs.add({
        id: r.id,
        user: r.user,
        action: r.action,
        message: r.message,
        timestamp: r.timestamp ? new Date(r.timestamp) : new Date(),
        entity: r.entity,
        entityId: r.entityId
      })
    );
    return logs;
  }
}

function cryptoRandom(): string {
  if (typeof crypto !== 'undefined' && 'getRandomValues' in crypto) {
    const buf = new Uint32Array(4);
    crypto.getRandomValues(buf);
    return Array.from(buf).map(n => n.toString(16)).join('');
  }
  return Math.random().toString(16).slice(2);
}
