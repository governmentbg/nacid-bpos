export enum ScrollType {
  top = 1,
  bottom = 2,
  middle = 3
}

export function parseScrollEvent(event: WheelEvent, target: HTMLElement) {
  const atBottom = target.scrollHeight - Math.ceil(target.scrollTop) === target.clientHeight;
  const atTop = Math.ceil(target.scrollTop) === 0;

  if (event.deltaY > 0 && atBottom)
    return ScrollType.bottom;
  else if (event.deltaY < 0 && atTop)
    return ScrollType.top;
  else
    return ScrollType.middle;
}

export function clone<T>(obj: T): T {
  if (!obj)
    return obj;
  return JSON.parse(JSON.stringify(obj));
}