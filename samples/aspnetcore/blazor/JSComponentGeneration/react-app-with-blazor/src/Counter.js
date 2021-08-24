import { useBlazor } from './blazor-react';

export function Counter({
  title,
  incrementAmount,
  customObject,
  customCallback,
}) {
  const fragment = useBlazor('counter', {
    title,
    incrementAmount,
    customObject,
    customCallback,
  });

  return fragment;
}
