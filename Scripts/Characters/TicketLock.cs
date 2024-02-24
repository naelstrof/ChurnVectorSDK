using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TicketLock {
    [System.Flags]
    public enum LockFlags {
        None = 0,
        Kinematic = 1,
        IgnoreCollisions = 2,
        Constraints = 4,
        FacingDirectionLock = 8,
        IgnoreUsables = 16,
        All = ~(0),
        Any = ~(FacingDirectionLock)
    }
    public delegate void LocksChangedAction(LockFlags flags);

    public event LocksChangedAction locksChanged;
    private List<Ticket> heldLocks;
    public bool GetLocked(LockFlags lockFlags = LockFlags.Any) {
        return (GetLockFlags() & lockFlags) != 0;
    }

    public TicketLock() {
        heldLocks = new List<Ticket>();
    }

    public class Ticket {
        private readonly MonoBehaviour owner;
        public readonly LockFlags lockFlags;
        public Ticket(MonoBehaviour owner, LockFlags lockFlags) {
            this.owner = owner;
            this.lockFlags = lockFlags;
        }
        public bool IsValid() => owner != null;
        public override string ToString() {
            return $"LockReason: {owner}, {lockFlags}";
        }
    }

    public Ticket AddLock(MonoBehaviour owner, LockFlags lockFlags = LockFlags.All) {
        Assert.AreNotEqual(LockFlags.None, lockFlags);
        heldLocks.Add(new Ticket(owner, lockFlags));
        OnLocksChanged(GetLockFlags());
        return heldLocks[^1];
    }

    void OnLocksChanged(LockFlags flags) {
        locksChanged?.Invoke(flags);
    }

    private LockFlags GetLockFlags() {
        LockFlags flags = LockFlags.None;
        foreach (var lockTicket in heldLocks) {
            flags |= lockTicket.lockFlags;
        }
        return flags;
    }

    public void RemoveLock(ref Ticket ticket) {
        heldLocks.Remove(ticket);
        OnLocksChanged(GetLockFlags());
        ticket = null;
    }
}
