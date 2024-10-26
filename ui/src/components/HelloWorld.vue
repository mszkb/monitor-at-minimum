<template>
  <v-container>
    <v-toolbar>
      <v-toolbar-title>Monitor</v-toolbar-title>

      <v-toolbar-items>
        <v-btn flat icon :active="intervalActive" @click="togglePing"><v-icon>mdi-access-point</v-icon></v-btn>
      </v-toolbar-items>
    </v-toolbar>

    <div class="d-flex justify-center">
      <v-progress-circular v-if="loading" indeterminate></v-progress-circular>
      <template v-else>
        <v-row dense class="justify-center">
          <v-col v-for="i in items" :key="i.id" cols="12" md="4" sm="6">
            <v-card hover>
              <v-card-item>
                <v-card-text>
                  <p class="mb-3">
                    <v-icon size="20" :color="coloring(i.status)" class="mr-2">
                      {{
                        i.status === Status.UP
                          ? "mdi-check-circle"
                          : "mdi-alert-circle"
                      }}
                    </v-icon>
                    <span>
                      <template v-if="i.status === Status.UP">{{ i.ping }}</template>
                      <template v-else>-</template>
                      &nbsp;ms&nbsp;
                      <v-icon v-if="i.status === Status.UP">mdi-access-point</v-icon>
                      <v-icon v-else>mdi-access-point-minus</v-icon>
                    </span>
                    {{ i.url }}
                  </p>
                  <span>Last checked: {{ new Date(i.lastChecked) }}</span>
                </v-card-text>

                <v-card-actions>
                  <v-btn color="red" @click="deleteTask(i.id)">Delete</v-btn>
                  <v-spacer></v-spacer>
                  <v-btn
                    :href="i.url"
                    target="_blank"
                    variant="outlined"
                    color="primary"
                    >Visit -></v-btn
                  >
                </v-card-actions>
              </v-card-item>
            </v-card>
          </v-col>
        </v-row>
      </template>
    </div>

    <v-btn @click="update">Update</v-btn>
  </v-container>
</template>

<script setup lang="ts">
import { Meta, Site, Status } from "@/types/data.type";
import { onMounted, ref } from "vue";


const loading = ref(false);
const items = ref([] as Site[]);
const meta = ref({} as Meta);

function coloring(status: Status) {
  return status === Status.UP ? "green" : "red";
}

onMounted(async () => {
  loading.value = true;

  await getMeta();
  await list();

  setTimeout(() => {
    loading.value = false;
  }, 1000);
  // loading.value = false;
});

async function getMeta() {
  try {
    const response = await fetch("http://localhost:3001/meta", {
      method: "GET",
    });

    meta.value = await response.json() as Meta;

    intervalActive.value = meta.value.intervalActive;
  } catch (error) {
    console.error(error);
  }
}

async function list() {
  try {
    const response = await fetch("http://localhost:3001/list", {
      method: "GET",
    });

    items.value = await response.json() as Site[];
  } catch (error) {
    console.error(error);
  }
}

async function update() {
  try {
    // set post request /update
    const response = await fetch("http://localhost:3001/update", {
      method: "POST",
    });

    items.value = await response.json() as Site[];
  } catch (error) {
    console.error(error);
  }
}

async function deleteTask(idx: number) {
  try {
    // set post request /delete
    const response = await fetch(`http://localhost:3001/delete/${idx}`, {
      method: "DELETE",
    });

    items.value = await response.json() as Site[];
  } catch (error) {
    console.error(error);
  }

  await list();
}

const intervalActive = ref(false);
async function togglePing() {
  intervalActive.value = !intervalActive.value;

  if (!intervalActive.value) {
    disableInterval();
  }

  try {
    // set post request /ping
    const response = await fetch("http://localhost:3001/ping", {
      method: "POST",
    });

    items.value = await response.json() as Site[];
  } catch (error) {
    console.error(error);
  }

  if (intervalActive.value) {
    enableInterval();
  }

  await list();
}

let interval: any;
function enableInterval() {
  if (interval) {
    clearInterval(interval);
  }

  if (!intervalActive.value) {
    return;
  }

  interval = setInterval(async () => {
    await list();
  }, 5000);
}

function disableInterval() {
  clearInterval(interval);
}
</script>
