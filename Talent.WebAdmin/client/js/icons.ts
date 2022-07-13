import { library, dom } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';

library.add(fas);
library.add(far);

// Reduce dll size by only importing icons which are actually being used:
// https://fontawesome.com/how-to-use/use-with-node-js
